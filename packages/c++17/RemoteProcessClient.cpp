#include <iostream>
#include "rapidjson/document.h"
#include "rapidjson/writer.h"
#include "rapidjson/stringbuffer.h"

#include "RemoteProcessClient.h"

using namespace std;
using namespace model;
using namespace rapidjson;

const int32 BUFFER_SIZE = 8 * 1024;

string RemoteProcessClient::readline() {
    while (true) {
        size_t eol = buffer.find('\n');
        if (eol != string::npos) {
            string line = buffer.substr(0, eol);
            buffer = buffer.substr(eol + 1);
            return line;
        }
        int32 received = socket.Receive(BUFFER_SIZE);
        if (received < 0) {
            cerr << "Error reading from socket" << endl;
            exit(10002);
        }
        if (received == 0) {
            return "";
        }
        buffer.append(socket.GetData(), socket.GetData() + received);
    }
}

void RemoteProcessClient::writeline(string line) {
    line.push_back('\n');
    if (socket.Send(reinterpret_cast<const uint8*>(line.c_str()), static_cast<int32_t>(line.length())) < 0) {
        cerr << "Failed to send data" << endl;
        exit(10003);
    }
}

RemoteProcessClient::RemoteProcessClient(string host, int port) {
    socket.Initialize();
    socket.DisableNagleAlgoritm();

    if (!socket.Open(reinterpret_cast<const uint8*>(host.c_str()), static_cast<int16>(port))) {
        cerr << "Failed to connect to " << host << ":" << port << endl;
        exit(10001);
    }

    writeline("json");
}

unique_ptr<Rules> RemoteProcessClient::read_rules() {
    string line = readline();
    if (line.empty()) {
        return unique_ptr<Rules>();
    }
    Document d;
    d.Parse(line.c_str());
    unique_ptr<Rules> result(new Rules());
    result->read(d);
    return result;
}

unique_ptr<Game> RemoteProcessClient::read_game() {
    string line = readline();
    if (line.empty()) {
        return unique_ptr<Game>();
    }
    Document d;
    d.Parse(line.c_str());
    unique_ptr<Game> result(new Game());
    result->read(d);
    return result;
}

void RemoteProcessClient::write(const unordered_map<int, Action>& actions, const string& custom_rendering) {
    Document d;
    d.SetObject();
    Document::AllocatorType& allocator = d.GetAllocator();
    for (auto it : actions) {
        d.AddMember(Value(to_string(it.first).c_str(), allocator).Move(), it.second.to_json(allocator).Move(), allocator);
    }
    StringBuffer buffer;
    Writer<StringBuffer> writer(buffer);
    d.Accept(writer);
    writeline(string(buffer.GetString()) + "|" + custom_rendering + "\n<end>");
}

void RemoteProcessClient::write_token(const string& token) {
    writeline(token);
}
