using OpenTK.Mathematics;

namespace NekinuSoft;

public class UI_Shader : ShaderProgram
{
    private int location_transformation;
    private int location_image_color;
    
    public UI_Shader(string vertex, string fragment) : base(vertex, fragment)
    {
    }

    public override void BindAttributes()
    {
        BindAttribute(0, "position");
    }

    public override void GetAllUniformLocations()
    {
        location_transformation = GetUniformLocation("transformation");
        location_image_color = GetUniformLocation("image_color");
    }

    public void Load_Color(Vector4 color)
    {
        Uniform4f(location_image_color, color);
    }

    public void Load_Transformartion(Matrix4 transform)
    {
        UniformMatrix4(location_transformation, transform);
    }
}