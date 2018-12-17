namespace Com.CodeGame.CodeBall2018.DevKit.CSharpCgdk.Model
{
    public sealed class Game
    {
        public int current_tick { get; set; }
        public Player[] players { get; set; }
        public Robot[] robots { get; set; }
        public NitroPack[] nitro_packs { get; set; }
        public Ball ball { get; set; }
    }
}