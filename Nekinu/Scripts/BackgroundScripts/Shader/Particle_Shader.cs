using NekinuSoft;
using OpenTK.Mathematics;
using Vector4 = NekinuSoft.Vector4;

public class Particle_Shader : ShaderProgram
{
    private int location_transformation;
    private int location_view;
    private int location_projection;

    private int location_particleColor;
    
    public Particle_Shader(string vertex, string fragment) : base(vertex, fragment)
    {
    }

    public override void BindAttributes()
    {
        BindAttribute(0, "position");
    }

    public override void GetAllUniformLocations()
    {
        location_transformation = GetUniformLocation("transformation");
        location_view = GetUniformLocation("view");
        location_projection = GetUniformLocation("projection");
        
        location_particleColor = GetUniformLocation("particle_color");
    }

    public void loadTransformation(Matrix4 transformation)
    {
        UniformMatrix4(location_transformation, transformation);
    }

    public void loadView(Matrix4 view)
    {
        UniformMatrix4(location_view, view);
    }

    public void loadProjection(Matrix4 projection)
    {
        UniformMatrix4(location_projection, projection);
    }

    public void loadColor(Vector4 color)
    {
        Uniform4f(location_particleColor, color);
    }
}