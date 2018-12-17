namespace Com.CodeGame.CodeBall2018.DevKit.CSharpCgdk.Model
{
    public class Vector
    {
        public double x { get; private set; }
        public double y { get; private set; }
        public double z { get; private set; }

        public Vector(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
