using NekinuSoft;

public class Batch
{
    //The shader that all the meshes will be rendered with. Not used currently
    public ShaderProgram Shader
    {
        get;
        private set;
    }

    //The mesh that will be rendered
    public Mesh Key
    {
        get;
        private set;
    }

    //The entities that are using the key mesh
    public List<Entity> entities;

    //Constructor
    public Batch(Mesh key)
    {
        entities = new List<Entity>();

        Key = key;
    }

    //Adds a entity to the Batch
    public void AddEntity(Entity entity)
    {
        entities.Add(entity);
    }

    //Check if a batch exists that uses a specific key (Mesh, 3D model)
    public static Batch BatchListHasKey(List<Batch> batches, Mesh key)
    {
        for (int i = 0; i < batches.Count; i++)
        {
            if (batches[i].Key == key)
            {
                return batches[i];
            }
        }

        return null;
    }
}