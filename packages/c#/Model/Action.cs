using Newtonsoft.Json;

namespace Com.CodeGame.CodeBall2018.DevKit.CSharpCgdk.Model
{
    public sealed class Action
    {
        public double target_velocity_x { get; set; }
        public double target_velocity_y { get; set; }
        public double target_velocity_z { get; set; }
        public double jump_speed { get; set; }
        public bool use_nitro { get; set; }

        [JsonIgnore]
        public Vector target_velocity
        {
            get { return new Vector(target_velocity_x, target_velocity_y, target_velocity_z); }
            set
            {
                target_velocity_x = value.x;
                target_velocity_y = value.y;
                target_velocity_z = value.z;
            }
        }
    }
}