namespace NekinuSoft
{
    public class Light : Component
    {
        public Vector4 color { get; set; }
        public float light_intensity { get; set; }

        public Vector3 light_attenuation { get; private set; }

        public Light(Vector4 color, Vector3 attenuation, float lightIntensity)
        {
            this.color = color;
            light_attenuation = attenuation;
            light_intensity = lightIntensity;
        }
    }
}