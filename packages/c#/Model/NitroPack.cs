namespace Com.CodeGame.CodeBall2018.DevKit.CSharpCgdk.Model
{
    public sealed class NitroPack : ISphere
    {
        public int id { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public double radius { get; set; }
        public double nitro_amount { get; set; }
        public int? respawn_ticks { get; set; }
    }
}