using Com.CodeGame.CodeBall2018.DevKit.CSharpCgdk.Model;

namespace Com.CodeGame.CodeBall2018.DevKit.CSharpCgdk
{
    public interface IStrategy
    {
        void Act(Robot me, Rules rules, Game game, Action action);
        string CustomRendering();
    }
}
