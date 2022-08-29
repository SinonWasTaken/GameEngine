using Nekinu_Soft;
using Nekinu_Soft.Mesh_Loader;
using Nekinu_Soft.Renderer;
using OpenTK.Graphics.ES11;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Image = Nekinu_Soft.UI.Image;

namespace Shader_Test.Scripts;

public class Basic_Renderer : IRenderer
{
    private Include_Shader_Test shader;

    private Entity test;

    private static Vector4 clip_plane;

    private Water_Framebuffer buffer;

    private Image reflection_image;
    private Image refraction_image;
    
    public Basic_Renderer()
    {
        buffer = new Water_Framebuffer();

        reflection_image = new Image("Reflection", buffer.Reflection_Texture, Vector4.one);
        refraction_image = new Image("Refraction", buffer.Refraction_Texture, Vector4.one);
        
        clip_plane = new Vector4(0, -1, 0, 0);
        
        shader = new Include_Shader_Test();

        test = new Entity("Yes", new Transform(new Vector3(0,-55, 5), Vector3.zero, new Vector3(50, 50, 50)));
        Mesh mesh = Mesh_Loader.loadOBJ(ResourceGetter.Get_Resource_File_Of_Type_String("Particle", ".obj"));
        test.AddComponent(mesh);
        
        test.Awake();
    }

    public override void Render(Camera camera)
    {
        test.Update();
        
        shader.Bind();

        /*GL.Enable(EnableCap.ClipPlane0);
        //place in a different renderer. Does nothing right now
        buffer.Bind_Reflection();
        float distance = 2 * camera.Parent.Transform.position.y - test.Transform.position.y;
        camera.Parent.Transform.position -= new Vector3(distance, 0, 0);
        camera.Parent.Transform.rotation = new Vector3(-camera.Parent.Transform.rotation.x, camera.Parent.Transform.rotation.y, camera.Parent.Transform.rotation.z);
        clip_plane = new Vector4(0, 1, 0, -test.Transform.position.y);
        R(camera);
        camera.Parent.Transform.rotation = new Vector3(camera.Parent.Transform.rotation.x, camera.Parent.Transform.rotation.y, camera.Parent.Transform.rotation.z);
        camera.Parent.Transform.position += new Vector3(distance, 0, 0);

        buffer.Bind_Refraction();
        clip_plane = new Vector4(0, -1, 0, test.Transform.position.y);
        R(camera);
        
        buffer.Unbind();
        
        clip_plane = new Vector4(0, 0, 0, 0);
        GL.Disable(EnableCap.ClipPlane0);*/
        R(camera);
        
        shader.Unbind();
    }

    private void R(Camera camera)
    {
        Mesh mesh = test.GetComponent<Mesh>();
        
        mesh.Bind();

        if (Input.is_key_down(Keys.Space))
        {
            camera.Parent.Transform.position += camera.Parent.Transform.up * Time.deltaTime;
        }

        if (Input.is_key_down(Keys.W))
        {
            camera.Parent.Transform.rotation += new Vector3(10 * Time.deltaTime, 0, 0);
        }
        
        if (Input.is_key_down(Keys.D))
        {
            camera.Parent.Transform.position -= camera.Parent.Transform.right * Time.deltaTime;
        }
        
        shader.Load_Plane(clip_plane);
        shader.Load_Matrix(test.TransformationMatrix, camera.View, camera.Projection);
        shader.Load_Camera_Position(camera.Parent.Transform.position);
        GL.DrawElements(PrimitiveType.Triangles, mesh.vertex_count, DrawElementsType.UnsignedInt, IntPtr.Zero);

        mesh.Unbind();
    }

    public static void Update_Clip_Plane(Vector4 clip)
    {
        clip_plane = clip;
    }

    public override void Close()
    {
        buffer.CleanUp();
    }
}