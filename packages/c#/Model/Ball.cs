namespace Com.CodeGame.CodeBall2018.DevKit.CSharpCgdk.Model
{
    public sealed class Ball : IMovableSphere
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public double velocity_x { get; set; }
        public double velocity_y { get; set; }
        public double velocity_z { get; set; }
        public double radius { get; set; }

        public Vector position => new Vector(x, y, z);
        public Vector velocity => new Vector(velocity_x, velocity_y, velocity_z);
    }
}