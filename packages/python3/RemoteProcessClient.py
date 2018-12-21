import socket
import io
import json
from model import *


class JSONEncoder(json.JSONEncoder):
    def default(self, obj):
        if isinstance(obj, Action):
            return obj.__dict__
        # Let the base class default method raise the TypeError
        return json.JSONEncoder.default(self, obj)


class RemoteProcessClient:
    def __init__(self, host, port):
        self.socket = socket.socket()
        self.socket.setsockopt(socket.IPPROTO_TCP, socket.TCP_NODELAY, True)
        self.socket.connect((host, port))
        self.reader = io.BufferedReader(
            socket.SocketIO(self.socket, 'r'))
        self.writer = self.socket.makefile('w')
        self.writer.write("json\n")
        self.writer.flush()

    def read_game(self):
        line = self.reader.readline()
        return Game(json.loads(line)) if line else None

    def read_rules(self):
        line = self.reader.readline()
        return Rules(json.loads(line)) if line else None

    def write(self, actions, custom_rendering):
        self.writer.write(json.dumps(
            actions, separators=(',', ':'), cls=JSONEncoder))
        self.writer.write('|')
        self.writer.write(custom_rendering)
        self.writer.write('\n<end>\n')
        self.writer.flush()

    def write_token(self, token):
        self.writer.write(token)
        self.writer.write('\n')
        self.writer.flush()
