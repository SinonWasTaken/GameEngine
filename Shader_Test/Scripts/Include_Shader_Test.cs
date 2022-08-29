using Nekinu_Soft;
using OpenTK.Mathematics;
using Vector3 = Nekinu_Soft.Vector3;
using Vector4 = Nekinu_Soft.Vector4;

namespace Shader_Test.Scripts
{
    public class Include_Shader_Test : ShaderProgram
    {
        private int transform;
        private int view;
        private int projection;

        private int cameraPosition;

        private int plane;
        
        public Include_Shader_Test() : base(ResourceGetter.Get_Resource_File_Of_Type_String("Test_Vertex", ".txt"), ResourceGetter.Get_Resource_File_Of_Type_String("Test_Fragment", ".txt"))
        {
        }

        public override void Bind_Attributes()
        {
            BindAttribute(0, "position");
            BindAttribute(1, "texture");
            BindAttribute(2, "normal");
        }

        public override void GetAllUniformLocations()
        {
            transform = GetUniformLocation("transform");
            view = GetUniformLocation("view");
            projection = GetUniformLocation("projection");

            cameraPosition = GetUniformLocation("cameraPosition");
            
            plane = GetUniformLocation("plane");
        }

        public void Load_Matrix(Matrix4 transform, Matrix4 view, Matrix4 projection)
        {
            UniformMatrix4(this.transform, transform);
            UniformMatrix4(this.view, view);
            UniformMatrix4(this.projection, projection);
        }

        public void Load_Camera_Position(Vector3 position)
        {
            Uniform3f(cameraPosition, position);
        }
        
        public void Load_Plane(Vector4 value)
        {
            Uniform4f(plane, value);
        }
    }
}