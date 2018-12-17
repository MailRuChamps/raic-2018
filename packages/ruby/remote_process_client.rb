require 'socket'
require 'json'
require './model/game'
require './model/rules'

class RemoteProcessClient
  def initialize(host, port)
    @socket = TCPSocket::new(host, port)
    write_line('json')
  end
  def write_line(line)
    @socket.puts(line)
    @socket.flush
  end
  def write_token(token)
    write_line(token)
  end
  def read_game()
    if (line = @socket.gets).nil?
      return nil
    end
    json = JSON.parse(line)
    return Game.new(json)
  end
  def read_rules()
    if (line = @socket.gets).nil?
      return nil
    end
    json = JSON.parse(line)
    return Rules.new(json)
  end
  def write(actions)
    write_line(actions.to_json)
  end
end