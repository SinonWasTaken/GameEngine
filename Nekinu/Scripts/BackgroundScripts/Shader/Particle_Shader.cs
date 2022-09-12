using NekinuSoft;
using OpenTK.Mathematics;
using Vector4 = NekinuSoft.Vector4;

//Default shader for particles
public class Particle_Shader : ShaderProgram
{
    //Location for all matrices in the shader
    private int location_transformation;
    private int location_view;
    private int location_projection;

    
    //location the color of the particles in the shader
    private int location_particleColor;
    
    public Particle_Shader(string vertex, string fragment) : base(vertex, fragment)
    {
    }

    //Binds the position of the particle
    public override void BindAttributes()
    {
        BindAttribute(0, "position");
    }

    //Loads all location pointers.
    public override void GetAllUniformLocations()
    {
        location_transformation = GetUniformLocation("transformation");
        location_view = GetUniformLocation("view");
        location_projection = GetUniformLocation("projection");
        
        location_particleColor = GetUniformLocation("particle_color");
    }

    //Used to load the entire transform of the particle into the shader. Contains information on 3d location, rotation and scale
    public void loadTransformation(Matrix4 transformation)
    {
        UniformMatrix4(location_transformation, transformation);
    }

    //Used to load the cameras transformation matrix
    public void loadView(Matrix4 view)
    {
        UniformMatrix4(location_view, view);
    }

    //Used to load the type of camera
    public void loadProjection(Matrix4 projection)
    {
        UniformMatrix4(location_projection, projection);
    }

    //Used to load the color of the particle into the shader
    public void loadColor(Vector4 color)
    {
        Uniform4f(location_particleColor, color);
    }
}