using System.Collections.Generic;
using Com.CodeGame.CodeBall2018.DevKit.CSharpCgdk.Model;

namespace Com.CodeGame.CodeBall2018.DevKit.CSharpCgdk
{
    public sealed class Runner
    {
        private readonly RemoteProcessClient remoteProcessClient;
        private readonly string token;
        public static void Main(string[] args)
        {
            new Runner(args.Length == 3 ? args : new[] { "127.0.0.1", "31001", "0000000000000000" }).Run();
        }

        private Runner(IReadOnlyList<string> args)
        {
            remoteProcessClient = new RemoteProcessClient(args[0], int.Parse(args[1]));
            token = args[2];
        }

        public void Run()
        {
            IStrategy strategy = new MyStrategy();
            IDictionary<int, Action> actions = new Dictionary<int, Action>();
            Game game;
            remoteProcessClient.WriteToken(token);
            Rules rules = remoteProcessClient.ReadRules();
            while ((game = remoteProcessClient.ReadGame()) != null)
            {
                actions.Clear();
                foreach (var robot in game.robots)
                {
                    if (robot.is_teammate)
                    {
                        Action action = new Action();
                        strategy.Act(robot, rules, game, action);
                        actions.Add(robot.id, action);
                    }
                }
                remoteProcessClient.Write(actions);
            }
        }
    }
}
