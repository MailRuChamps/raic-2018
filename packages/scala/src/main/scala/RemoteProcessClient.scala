import java.io._
import java.net.{InetSocketAddress, Socket}
import java.nio.charset.StandardCharsets
import scala.collection.JavaConverters._

import com.google.gson.Gson
import model.{Action, Game, Rules}

class RemoteProcessClient(host: String, port: Int) {
  val socket: Socket = new Socket()
  socket.setTcpNoDelay(true)
  socket.connect(new InetSocketAddress(host, port))
  val gson = new Gson()

  val input: BufferedReader = new BufferedReader(
    new InputStreamReader(new BufferedInputStream(socket.getInputStream), StandardCharsets.UTF_8))
  val output: OutputStreamWriter = new OutputStreamWriter(new BufferedOutputStream(socket.getOutputStream), StandardCharsets.UTF_8)

  output.write("json\n")
  output.flush()

  def readGame(): Game = {
    val line = input.readLine()
    gson.fromJson(line, classOf[Game])
  }

  def readRules(): Rules = {
    val line = input.readLine()
    gson.fromJson(line, classOf[Rules])
  }

  def write(actions: Map[Int, Action]): Unit = {
    val json = gson.toJson(actions.asJava)
    output.write(json)
    output.write("\n")
    output.flush()
  }

  def writeToken(token: String): Unit = {
    output.write(token)
    output.write("\n")
    output.flush()
  }
}
