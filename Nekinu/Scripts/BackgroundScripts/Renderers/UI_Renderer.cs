using NekinuSoft;
using NekinuSoft.Renderer;
using NekinuSoft.Scene_Manager;
using NekinuSoft.UI;
using OpenTK.Graphics.ES11;

public class UI_Renderer : IRenderer
{
    private UI_Shader shader;
    
    public UI_Renderer()
    {
        shader = new UI_Shader(ResourceGetter.Get_Resource_File_Of_Type_String("UI_Vertex", ".txt"), ResourceGetter.Get_Resource_File_Of_Type_String("UI_Fragment", ".txt"));
    }
    
    public override void Render(Camera camera)
    {
        GL.Enable(EnableCap.Blend);
        GL.Disable(EnableCap.DepthTest);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        List<Entity> uis = getUIEntities();

        for (int i = 0; i < uis.Count; i++)
        {
            UI_Class image = uis[i].GetComponent<UI_Class>();
            
            image.Is_Mouse_Over(camera);
            
            image.UiMesh.Bind();

            shader.Bind();
            
            if (image.UiTexture != null)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, image.UiTexture.id);
            }

            shader.Load_Transformartion(Matrix4x4.Create_UI_Matrix(uis[i]));
            shader.Load_Color(image.Color);
            
            GL.DrawElements(PrimitiveType.Triangles, image.UiMesh.vertex_count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            
            GL.BindTexture(TextureTarget.Texture2D, 0);
            
            shader.Unbind();
            
            image.UiMesh.Unbind();

            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);
        }
    }

    private List<Entity> getUIEntities()
    {
        List<Entity> ui = new List<Entity>();

        for (int i = 0; i < SceneManager.GetSceneEntities().Count; i++)
        {
            Entity entity = SceneManager.GetSceneEntities()[i];
            if (entity.GetComponent<UI_Class>() != null)
            {
                if (entity.IsActive)
                {
                    ui.Add(entity);
                }
            }
        }

        return ui;
    }
}