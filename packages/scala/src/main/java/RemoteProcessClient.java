import com.google.gson.Gson;
import model.*;

import java.io.*;
import java.net.InetSocketAddress;
import java.net.Socket;
import java.nio.charset.StandardCharsets;
import java.util.Map;

public final class RemoteProcessClient {
    BufferedReader input;
    OutputStreamWriter output;
    Gson gson = new Gson();

    public RemoteProcessClient(String host, int port) throws IOException {
        Socket socket = new Socket();
        socket.setTcpNoDelay(true);
        socket.connect(new InetSocketAddress(host, port));

        input = new BufferedReader(
                new InputStreamReader(new BufferedInputStream(socket.getInputStream()), StandardCharsets.UTF_8));
        output = new OutputStreamWriter(new BufferedOutputStream(socket.getOutputStream()), StandardCharsets.UTF_8);

        output.write("json\n");
        output.flush();
    }

    public Game readGame() throws IOException {
        String line = input.readLine();
        return gson.fromJson(line, Game.class);
    }

    public Rules readRules() throws IOException {
        String line = input.readLine();
        return gson.fromJson(line, Rules.class);
    }

    public void write(Map<Integer, Action> actions) throws IOException {
        String json = gson.toJson(actions);
        output.write(json);
        output.write("\n");
        output.flush();
    }

    public void writeToken(String token) throws IOException {
        output.write(token);
        output.write("\n");
        output.flush();
    }
}