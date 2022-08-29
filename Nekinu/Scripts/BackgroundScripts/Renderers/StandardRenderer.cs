using NekinuSoft;
using NekinuSoft.Renderer;
using NekinuSoft.Scene_Manager;
using OpenTK.Graphics.ES30;

public class StandardRenderer : IRenderer
{
    private List<Batch> batches;
    
    public StandardRenderer()
    {
        batches = new List<Batch>();
    }
    
    public override void Render(Camera camera)
    {
        Sort_Batches();

        for (int i = 0; i < batches.Count; i++)
        {
            Mesh key = batches[i].Key;

            key.Bind();
            
            for (int j = 0; j < batches[i].entities.Count; j++)
            {
                GL.DrawElements(PrimitiveType.Triangles, key.vertex_count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            }
            
            key.Unbind();
        }
    }

    private void Sort_Batches()
    {
        for (int i = 0; i < SceneManager.GetSceneEntities().Count; i++)
        {
            Mesh mesh = SceneManager.GetSceneEntities()[i].GetComponent<Mesh>();

            if (mesh != null)
            {
                Batch batch = Batch.BatchListHasKey(batches, mesh);

                if (batch == null)
                {
                    Batch new_batch = new Batch(mesh);
                    new_batch.AddEntity(SceneManager.GetSceneEntities()[i]);
                    batches.Add(new_batch);
                }
                else
                {
                    batch.AddEntity(SceneManager.GetSceneEntities()[i]);
                }
            }
        }
    }
}