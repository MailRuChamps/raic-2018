#[macro_use]
extern crate log;
extern crate env_logger;
extern crate serde;
#[macro_use]
extern crate serde_derive;
extern crate serde_json;

mod model;
mod my_strategy;
mod remote_process_client;
mod strategy;

use crate::remote_process_client::RemoteProcessClient;
use crate::strategy::Strategy;
use std::io;

struct Args {
    host: String,
    port: u16,
    token: String,
    my_name: String,
}

fn main() {
    env_logger::init();

    use crate::my_strategy::MyStrategy;
    use std::io::{stderr, Write};
    use std::process::exit;

    let args = parse_args();

    let client = match RemoteProcessClient::connect(&args.host[..], args.port) {
        Ok(v) => v,
        Err(v) => {
            write!(&mut stderr(), "{:?}\n", v).unwrap();
            exit(-1);
        }
    };

    let mut runner = Runner::new(client, args.token, args.my_name);

    match runner.run::<MyStrategy>() {
        Ok(_) => (),
        Err(v) => {
            write!(&mut stderr(), "{:?}\n", v).unwrap();
            exit(-1);
        }
    }
}

fn parse_args() -> Args {
    use std::process::exit;

    if std::env::args().count() == 5 {
        Args {
            my_name: std::env::args().nth(1).unwrap(),
            host: std::env::args().nth(2).unwrap(),
            port: std::env::args()
                .nth(3)
                .unwrap()
                .parse()
                .expect("Cant't parse port"),
            token: std::env::args().nth(4).unwrap(),
        }
    } else if std::env::args().count() == 2 {
        Args {
            my_name: std::env::args().nth(1).unwrap(),
            host: "127.0.0.1".to_string(),
            port: 31001,
            token: "0000000000000000".to_string(),
        }
    } else {
        eprintln!("Usage: <my_name> [<host> <port> <token>]");
        exit(1);
    }
}

struct Runner {
    client: RemoteProcessClient,
    token: String,
    my_name: String,
}

impl Runner {
    pub fn new(client: RemoteProcessClient, token: String, my_name: String) -> Self {
        Runner { client, token, my_name }
    }

    pub fn run<T: Strategy + Default>(&mut self) -> io::Result<()> {
        use std::io::BufRead;

        let mut strategy = T::default();
        self.client.write_token(&self.token)?;
        self.client.read::<model::Rules>()?.expect("No rules");
        let mut buffer = String::new();
        let stdin = std::io::stdin();
        let mut locked_stdin = stdin.lock();
        locked_stdin.read_line(&mut buffer)?;
        let rules = ::serde_json::from_str(&buffer)?;
        let mut actions = std::collections::HashMap::new();
        let mut meta = None;
        while let Some(_) = self.client.read::<model::Game>()? {
            buffer.clear();
            locked_stdin.read_line(&mut buffer)?;
            let record: Record = ::serde_json::from_str(&buffer)?;
            let game = record.as_model_game(&self.my_name, &mut meta)?;
            actions.clear();
            for robot in game.robots.iter().filter(|robot| robot.is_teammate) {
                strategy.act(robot, &rules, &game, &mut model::Action::default());
                actions.insert(robot.id, model::Action::default());
            }
            debug!("{:?}", actions);
            self.client.write(&actions, &strategy.custom_rendering())?;
            debug!("Written");
        }

        Ok(())
    }
}

struct Meta {
    players: Vec<Player>,
    opposite_coordinates: bool,
    nitro_packs: Vec<(Vec3, i32)>,
}

struct Player {
    index: usize,
    base: model::Player,
}

#[derive(Debug, Deserialize)]
struct Record {
    current_tick: i32,
    ticks_per_second: i32,
    names: Option<Vec<String>>,
    scores: Vec<i32>,
    robots: Vec<Robot>,
    nitro_packs: Vec<NitroPack>,
    ball: Ball,
    reset_ticks: Option<i32>,
}

impl Record {
    pub fn as_model_game(&self, my_name: &String, meta: &mut Option<Meta>) -> io::Result<model::Game> {
        if meta.is_none() {
            if !self.names.as_ref().unwrap().contains(&my_name) {
                return Err(io::Error::new(io::ErrorKind::Other, InvalidMyName));
            }
            *meta = Some(Meta {
                players: self.names.as_ref().unwrap().iter().enumerate()
                    .map(|(n, v)| (
                        Player {
                            index: n,
                            base: model::Player {
                                id: n as i32 + 1,
                                me: v == my_name,
                                strategy_crashed: false,
                                score: self.scores[n],
                            },
                        }
                    ))
                    .collect(),
                opposite_coordinates: self.names.as_ref().unwrap()[1] == *my_name,
                nitro_packs: self.nitro_packs.iter().enumerate()
                    .map(|(n, v)| (v.position, n as i32 + 1))
                    .collect(),
            });
        }
        let meta = meta.as_ref().unwrap();
        Ok(model::Game {
            current_tick: self.current_tick,
            players: meta.players.iter()
                .map(|v| v.base.clone())
                .collect(),
            robots: self.robots.iter()
                .map(|v| v.as_model_robot(meta.opposite_coordinates, &meta.players))
                .collect(),
            nitro_packs: self.nitro_packs.iter()
                .map(|v| v.as_model_nitro_pack(meta.opposite_coordinates, &meta.nitro_packs))
                .collect(),
            ball: self.ball.as_model_ball(meta.opposite_coordinates),
        })
    }
}

#[derive(Debug)]
struct InvalidMyName;

impl std::error::Error for InvalidMyName {
    fn description(&self) -> &str {
        "invalid my name"
    }
}

impl std::fmt::Display for InvalidMyName {
    fn fmt(&self, f: &mut std::fmt::Formatter) -> std::fmt::Result {
        write!(f, "{:?}", self)
    }
}

#[derive(Debug, Deserialize)]
struct Robot {
    id: i32,
    player_index: usize,
    position: Vec3,
    velocity: Vec3,
    radius: f64,
    nitro: f64,
    last_touch: Option<Vec3>,
}

impl Robot {
    pub fn as_model_robot(&self, opposite_coordinates: bool, players: &Vec<Player>) -> model::Robot {
        let player = players.iter()
            .find(|v| v.index == self.player_index);

        let (position, velocity, last_touch) = if opposite_coordinates {
            (self.position.opposite(), self.velocity.opposite(), self.last_touch.map(|v| v.opposite()))
        } else {
            (self.position, self.velocity, self.last_touch)
        };

        model::Robot {
            id: self.id,
            player_id: player.map(|v| v.base.id).unwrap(),
            is_teammate: player.map(|v| v.base.me).unwrap(),
            x: position.x,
            y: position.y,
            z: position.z,
            velocity_x: velocity.x,
            velocity_y: velocity.y,
            velocity_z: velocity.z,
            radius: self.radius,
            nitro_amount: self.nitro,
            touch: last_touch.is_some(),
            touch_normal_x: last_touch.map(|v| v.x),
            touch_normal_y: last_touch.map(|v| v.y),
            touch_normal_z: last_touch.map(|v| v.z),
        }
    }
}

#[derive(Debug, Deserialize)]
struct NitroPack {
    position: Vec3,
    radius: f64,
    nitro_amount: f64,
    respawn_ticks: Option<i32>,
}

impl NitroPack {
    pub fn as_model_nitro_pack(&self, opposite_coordinates: bool, nitro_packs: &Vec<(Vec3, i32)>) -> model::NitroPack {
        let position = if opposite_coordinates {
            self.position.opposite()
        } else {
            self.position
        };

        model::NitroPack {
            id: nitro_packs.iter()
                .find(|(position, _)| *position == self.position)
                .map(|(_, id)| *id)
                .unwrap(),
            x: position.x,
            y: position.y,
            z: position.z,
            radius: self.radius,
            nitro_amount: self.nitro_amount,
            respawn_ticks: self.respawn_ticks,
        }
    }
}

#[derive(Debug, Deserialize)]
struct Ball {
    position: Vec3,
    velocity: Vec3,
    radius: f64,
}

impl Ball {
    pub fn as_model_ball(&self, opposite_coordinates: bool) -> model::Ball {
        let (position, velocity) = if opposite_coordinates {
            (self.position.opposite(), self.velocity.opposite())
        } else {
            (self.position, self.velocity)
        };

        model::Ball {
            x: position.x,
            y: position.y,
            z: position.z,
            velocity_x: velocity.x,
            velocity_y: velocity.y,
            velocity_z: velocity.z,
            radius: self.radius,
        }
    }
}

#[derive(Debug, Copy, Clone, Deserialize, PartialEq)]
struct Vec3 {
    x: f64,
    y: f64,
    z: f64,
}

impl Vec3 {
    pub fn opposite(&self) -> Vec3 {
        Vec3 {
            x: self.x,
            y: self.y,
            z: -self.z,
        }
    }
}
