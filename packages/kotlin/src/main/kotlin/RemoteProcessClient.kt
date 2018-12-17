import com.google.gson.Gson
import model.*

import java.io.*
import java.net.InetSocketAddress
import java.net.Socket
import java.nio.charset.StandardCharsets

class RemoteProcessClient @Throws(IOException::class)
constructor(host: String, port: Int) {
    internal var input: BufferedReader
    internal var output: OutputStreamWriter
    internal var gson = Gson()

    init {
        val socket = Socket()
        socket.tcpNoDelay = true
        socket.connect(InetSocketAddress(host, port))

        input = BufferedReader(
                InputStreamReader(BufferedInputStream(socket.getInputStream()), StandardCharsets.UTF_8))
        output = OutputStreamWriter(BufferedOutputStream(socket.getOutputStream()), StandardCharsets.UTF_8)

        output.write("json\n")
        output.flush()
    }

    @Throws(IOException::class)
    fun readGame(): Game? {
        val line = input.readLine()
        return gson.fromJson(line, Game::class.java)
    }

    @Throws(IOException::class)
    fun readRules(): Rules {
        val line = input.readLine()
        return gson.fromJson(line, Rules::class.java)
    }

    @Throws(IOException::class)
    fun write(actions: Map<Int, Action>) {
        val json = gson.toJson(actions)
        output.write(json)
        output.write("\n")
        output.flush()
    }

    @Throws(IOException::class)
    fun writeToken(token: String) {
        output.write(token)
        output.write("\n")
        output.flush()
    }
}