using Nekinu;

namespace NekinuSoft
{
    public class Spot_Light : Light
    {
        [SerializedProperty] private float light_range;

        public Spot_Light() : base(new Vector4(1,1,1,1), new Vector3(1,1,1), 1)
        {
            light_range = 1;
        }

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