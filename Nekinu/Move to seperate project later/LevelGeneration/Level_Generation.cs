using NekinuSoft;

public class Level_Generation
{
    /*public int y_island_spawn = 65;
    
    public int map_size;

    public int y_min, y_max;
    public int chunk_Size;

    public float distance_threshold;

    public List<Vector3> positions;

    private Chunk_Data data;

    private World world;

    public List<Noise_Values> noise_values;
    
    public Level_Generation(Chunk_Data data, World world)
    {
        this.data = data;
        this.world = world;
        
        positions = new List<Vector3>();
    }
    
    public void Generate_Island_At(Vector2 start, Vector2 end)
    {
        noise_values = new List<Noise_Values>();

        map_size = Chunk_Data.map_size;

        y_max = data.y_max;
        y_min = data.y_min;

        y_island_spawn = data.y_spawn;

        distance_threshold = data.distance_threshold;
        chunk_Size = Chunk_Data.chunk_Size;

        do_perlin_noise(start, end);
    }
    
    private void do_perlin_noise(Vector2 start, Vector2 end)
    {
        for (int x = (int) start.x; x <= (int) end.x; x++)
        {
            for (int y = (int) start.y; y <= (int) end.y; y++)
            {
                int value = world.get_noise(x, y, y_max - y_min, chunk_Size);

                float v = value / 20f;
                
                noise_values.Add(new Noise_Values(x, y, v));
            }
        }
        
        spawn_blocks(start, end);
    }
    
    private void spawn_blocks(Vector2 start, Vector2 end)
    {
        for (int x = (int) start.x; x <= (int) end.x; x++)
        {
            for (int y = (int) start.y; y <= (int) end.y; y++)
            {
                float value = get_noise_value(x, y);

                if (value != 0)
                {
                    int y_Value = (int) (Math.Round(value * 10f));

                    positions.Add(new Vector3(x, y_island_spawn + y_Value, y));
                }
            }
        }
    }

    private float get_noise_value(int x, int y)
    {
        for (int i = 0; i < noise_values.Count; i++)
        {
            if (noise_values[i].x == x && noise_values[i].y == y)
                return noise_values[i].value;
        }

        return -1;
    }*/
}

public class Noise_Values
{
    public int x { get; private set; }
    public int y { get; private set; }
    
    public float value { get; private set; }

    public Noise_Values(int x, int y, float value)
    {
        this.x = x;
        this.y = y;
        this.value = value;
    }
}