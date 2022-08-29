namespace NekinuSoft;

public class LitShader : Shader
{
    private readonly int LightCount = 4;

    private int[] light_location;
    
    public LitShader(string vertex, string fragment) : base(vertex, fragment)
    {
    }

    public override void BindAttributes()
    {
        base.BindAttributes();
    }

    public override void GetAllUniformLocations()
    {
        base.GetAllUniformLocations();

        light_location = new int[LightCount];

        for (int i = 0; i < LightCount; i++)
        {
            light_location[i] = GetUniformLocation("lights[" + i + "]");
        }
    }

    public override void LoadLights(Entity render_object)
    {
        List<Light> lights = GetSceneLights();

        lights = new List<Light>(lights.OrderByDescending(i => Vector3.Distance(render_object.Transform.position, i.Parent.Transform.position)));
        
        for (int i = 0; i < lights.Count; i++)
        {
            Console.WriteLine($"{Vector3.Distance(render_object.Transform.position, lights[i].Parent.Transform.position)}");
        }

        while (lights.Count < LightCount)
        {
            lights.Add(new Light(new Vector4(1,1,1,1), new Vector3(1, 0.1f, 0.01f), 0));
        }
    }
}