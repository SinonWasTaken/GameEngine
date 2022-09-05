using Nekinu;

namespace NekinuSoft
{
    public class Spot_Light : Light
    {
        //Modifies how far the light goes
        [SerializedProperty] private float light_range;

        //Default constructor
        public Spot_Light() : base(new Vector4(1,1,1,1), new Vector3(1,1,1), 1)
        {
            light_range = 1;
        }

        //Set the light range
        public Spot_Light(float light_range, Vector4 color, Vector3 attentuation, float lightIntensity) : base(color, attentuation, lightIntensity)
        {
            this.light_range = light_range;
        }

        public float LightRange
        {
            get => light_range;
            set => light_range = value;
        }
    }
}