using System.Net.Sockets;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Com.CodeGame.CodeBall2018.DevKit.CSharpCgdk
{
    public sealed class RemoteProcessClient
    {
        TcpClient client;
        StreamReader reader;
        StreamWriter writer;
        public RemoteProcessClient(string host, int port)
        {
            client = new TcpClient(host, port) { NoDelay = true };
            reader = new StreamReader(client.GetStream());
            writer = new StreamWriter(client.GetStream());

            writer.WriteLine("json");
            writer.Flush();
        }

        public Model.Game ReadGame()
        {
            string line = reader.ReadLine();
            if (line == null || line.Length == 0)
            {
                return null;
            }
            else
            {
                return JsonConvert.DeserializeObject<Model.Game>(line);
            }
        }

        public Model.Rules ReadRules()
        {
            string line = reader.ReadLine();
            if (line == null || line.Length == 0)
            {
                return null;
            }
            else
            {
                return JsonConvert.DeserializeObject<Model.Rules>(line);
            }
        }

        public void Write(IDictionary<int, Model.Action> actions, string custom_rendering)
        {
            writer.WriteLine(JsonConvert.SerializeObject(actions) + "|" + custom_rendering + "\n<end>");
            writer.Flush();
        }

        public void WriteToken(string token)
        {
            writer.WriteLine(token);
            writer.Flush();
        }
    }
}