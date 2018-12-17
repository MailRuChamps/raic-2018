namespace Com.CodeGame.CodeBall2018.DevKit.CSharpCgdk.Model
{
    public interface IMovableSphere : ISphere
    {
        double velocity_x { get; }
        double velocity_y { get; }
        double velocity_z { get; }

        Vector velocity { get; }
    }
}
