using NekinuSoft;
using Math = NekinuSoft.Math.Math;

public class Floating_Island_Generator
{
    /*public int y_island_spawn = 65;
    
    public int map_size;

    public float[,] noise_values;

    public int y_min, y_max;
    public int chunk_Size;

    public float distance_threshold;

    public List<Vector3> positions;

    private Chunk_Data data;

    private World world;

    private int smooth_count;
    
    public Floating_Island_Generator(Chunk_Data data, World world)
    {
        this.data = data;
        this.world = world;
    }

    public void Generate_Island_At()
    {
        smooth_count = 10;
        
        positions = new List<Vector3>();
        
        map_size = 100;

        y_max = data.y_max;
        y_min = data.y_min;

        y_island_spawn = data.y_spawn;

        distance_threshold = data.distance_threshold;
        chunk_Size = Chunk_Data.chunk_Size;
        
        noise_values = new float[map_size, map_size];

        do_perlin_noise();
    }

    private void do_perlin_noise()
    {
        for (int x = 0; x < map_size; x++)
        {
            for (int y = 0; y < map_size; y++)
            {
                int value = world.get_noise(x, y, y_max - y_min , chunk_Size);

                float v = value / 20f;

                noise_values[x, y] = v;
            }
        }
        
        do_gradient_remove();
    }
    
    private void do_gradient_remove()
    {
        for (int x = 0; x < map_size; x++)
        {
            for (int y = 0; y < map_size; y++)
            {
                float c = noise_values[x, y];

                float distx = ((map_size / 2f) - x) * ((map_size / 2f) - x);
                float disty = ((map_size / 2f) - y) * ((map_size / 2f) - y);

                float distance = (float)System.Math.Sqrt(distx + disty) / (map_size * 1f);

                float new_value = c - distance;

                if (new_value > distance_threshold)
                {
                    if (new_value > 1)
                    {
                        noise_values[x, y] = 1;
                    }
                    else
                    {
                        noise_values[x, y] = c;
                    }
                }
                else
                {

                    noise_values[x, y] = 0;
                }
            }
        }
        
        SmoothMap();
        
        spawn_blocks();
    }
    
    void SmoothMap()
    {
        for (int i = 0; i < smooth_count; i++)
        {
            for (int x = 0; x < map_size; x++)
            {
                for (int y = 0; y < map_size; y++)
                {
                    int surrounding_count = Get_Surrounding_Blocks(x, y);
                    
                    if (surrounding_count < 4)
                    {
                        noise_values[x, y] = 0;
                    }
                }
            }
        }
    }
    
    int Get_Surrounding_Blocks(int x, int y)
    {
        int count = 0;

        for (int grid_x = x - 1; grid_x <= x + 1; grid_x++)
        {
            for (int grid_y = y - 1; grid_y <= y + 1; grid_y++)
            {
                if (grid_x >= 0 && grid_x < map_size && grid_y >= 0 && grid_y < map_size)
                {
                    if (grid_x != x || grid_y != y)
                    {
                        if (noise_values[grid_x, grid_y] != 0)
                        {
                            count++;
                        }
                    }
                }
            }
        }
        
        return count;
    }
    
    private void spawn_blocks()
    {
        for (int x = 0; x < map_size; x++)
        {
            for (int y = 0; y < map_size; y++)
            {
                float value = noise_values[x, y];

                if (value != 0)
                {
                    int y_Value = (Math.Round(value * 10f));

                    positions.Add(new Vector3(x, y_island_spawn + y_Value, y));
                }
            }
        }
    }*/
}