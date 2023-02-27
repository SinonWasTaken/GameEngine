using OpenTK.Mathematics;

namespace NekinuSoft;

public class AnimatedModelShader : ShaderProgram
{
    private const int MAX_JOINTS = 50;

    private int projectionMatrix;
    private int lightDirection;
    private int[] jointTransforms;
    
    public AnimatedModelShader(string vertex, string fragment) : base(vertex, fragment)
    {
    }
    
    public override void BindAttributes()
    {
        BindAttribute(0, "position");
        BindAttribute(1, "texture");
        BindAttribute(2, "normal");
        BindAttribute(3, "jointID");
        BindAttribute(4, "jointWeight");
    }

    //Loads all location pointers.
    public override void GetAllUniformLocations()
    {
        projectionMatrix = GetUniformLocation("projection");
        lightDirection = GetUniformLocation("lightDirection");
    }

    public void LoadProjection(Matrix4 projection)
    {
        UniformMatrix4(projectionMatrix, projection);
    }
}