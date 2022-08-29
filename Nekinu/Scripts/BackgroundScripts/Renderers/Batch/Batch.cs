using NekinuSoft;

public class Batch
{
    public ShaderProgram Shader
    {
        get;
        private set;
    }

    public Mesh Key
    {
        get;
        private set;
    }

    public List<Entity> entities;

    public Batch(Mesh key)
    {
        entities = new List<Entity>();

        Key = key;
    }

    public void AddEntity(Entity entity)
    {
        entities.Add(entity);
    }

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