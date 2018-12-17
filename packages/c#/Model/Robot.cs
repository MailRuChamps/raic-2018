namespace Com.CodeGame.CodeBall2018.DevKit.CSharpCgdk.Model
{
    public sealed class Robot
    {
        public int id;
        public int player_id;
        public bool is_teammate;
        public double x;
        public double y;
        public double z;
        public double velocity_x;
        public double velocity_y;
        public double velocity_z;
        public double radius;
        public double nitro_amount;
        public bool touch;
        public double? touch_normal_x;
        public double? touch_normal_y;
        public double? touch_normal_z;
    }
}