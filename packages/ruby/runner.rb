require './my_strategy'
require './remote_process_client'
require './model/action'

class Runner
  def initialize
    if ARGV.length == 3
      @remote_process_client = RemoteProcessClient::new(ARGV[0], ARGV[1].to_i)
      @token = ARGV[2]
    else
      @remote_process_client = RemoteProcessClient::new('127.0.0.1', 31001)
      @token = '0000000000000000'
    end
  end

  def run
    begin
      strategy = MyStrategy::new
      
      @remote_process_client.write_token(@token)
      rules = @remote_process_client.read_rules()
      until (game = @remote_process_client.read_game).nil?
        actions = {}
        for robot in game.robots
          if robot.is_teammate
            action = Action::new
            strategy.act(robot, rules, game, action)
            actions[robot.id] = action
          end
        end

        @remote_process_client.write(actions)
      end
    end
  end
end

Runner.new.run