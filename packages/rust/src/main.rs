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

use remote_process_client::RemoteProcessClient;
use std::io;
use strategy::Strategy;

struct Args {
    host: String,
    port: u16,
    token: String,
}

fn main() {
    env_logger::init();

    use my_strategy::MyStrategy;
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

    let mut runner = Runner::new(client, args.token);

    match runner.run::<MyStrategy>() {
        Ok(_) => (),
        Err(v) => {
            write!(&mut stderr(), "{:?}\n", v).unwrap();
            exit(-1);
        }
    }
}

fn parse_args() -> Args {
    if std::env::args().count() == 4 {
        Args {
            host: std::env::args().nth(1).unwrap(),
            port: std::env::args()
                .nth(2)
                .unwrap()
                .parse()
                .expect("Cant't parse port"),
            token: std::env::args().nth(3).unwrap(),
        }
    } else {
        Args {
            host: "127.0.0.1".to_string(),
            port: 31001,
            token: "0000000000000000".to_string(),
        }
    }
}

struct Runner {
    client: RemoteProcessClient,
    token: String,
}

impl Runner {
    pub fn new(client: RemoteProcessClient, token: String) -> Self {
        Runner { client, token }
    }

    pub fn run<T: Strategy + Default>(&mut self) -> io::Result<()> {
        let mut strategy = T::default();
        self.client.write_token(&self.token)?;
        let rules = self.client.read::<model::Rules>()?.expect("No rules");
        let mut actions = std::collections::HashMap::new();
        while let Some(game) = self.client.read::<model::Game>()? {
            actions.clear();
            for robot in game.robots.iter().filter(|robot| robot.is_teammate) {
                let mut action = model::Action::default();
                strategy.act(robot, &rules, &game, &mut action);
                actions.insert(robot.id, action);
            }
            debug!("{:?}", actions);
            self.client.write(&actions)?;
            debug!("Written");
        }

        Ok(())
    }
}
