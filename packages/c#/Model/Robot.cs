namespace Com.CodeGame.CodeBall2018.DevKit.CSharpCgdk.Model
{
    public sealed class Robot : ISphere
    {
        public int id { get; set; }
        public int player_id { get; set; }
        public bool is_teammate { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public double velocity_x { get; set; }
        public double velocity_y { get; set; }
        public double velocity_z { get; set; }
        public double radius { get; set; }
        public double nitro_amount { get; set; }
        public bool touch { get; set; }
        public double? touch_normal_x { get; set; }
        public double? touch_normal_y { get; set; }
        public double? touch_normal_z { get; set; }
    }
}