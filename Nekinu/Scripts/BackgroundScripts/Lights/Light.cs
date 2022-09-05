namespace NekinuSoft
{
    //Default light class
    public class Light : Component
    {
        //Color of the light
        public Vector4 color { get; set; }
        //How strong the light is
        public float light_intensity { get; set; }

        //How far the light travels
        public Vector3 light_attenuation { get; private set; }

        ////Default constructor
        public Light(Vector4 color, Vector3 attenuation, float lightIntensity)
        {
            this.color = color;
            light_attenuation = attenuation;
            light_intensity = lightIntensity;
        }
    }
}