using NekinuSoft;
using NekinuSoft.MeshLoader;
using OpenTK.Mathematics;
using Vector3 = NekinuSoft.Vector3;
using Vector4 = NekinuSoft.Vector4;

public class Particle
{
    private Mesh particle_mesh;

    private float total_life_time;
    private float current_life_time;
    private float speed;
    
    private Vector3 velocity;
    
    private Vector3 position;
    private Vector3 rotation;
    private Vector3 scale;

    private float gravity_scale;

    private Vector4 color;
    
    public Matrix4 transformation_matrix;

    public Particle()
    {
        
    }
    
    public Particle(string mesh_name, float totalLifeTime, float speed, Vector3 velocity, Vector3 position, Vector3 rotation, Vector3 scale, float gravity, Vector4 color)
    {
        particle_mesh = MeshLoader.loadOBJ(mesh_name);
        
        total_life_time = totalLifeTime;

        current_life_time = totalLifeTime;

        this.speed = speed;
        
        this.velocity = velocity;
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
        gravity_scale = gravity;

        this.color = color;
    }

    public bool Update()
    {
        velocity += new Vector3(0, -50f * speed * gravity_scale * Time.deltaTime, 0);
        
        Vector3 change = new Vector3(velocity.x, velocity.y, velocity.z);
        change *= Time.deltaTime;
        
        position += change;

        transformation_matrix = Matrix4x4.cameraTransformationMatrix(new Transform(position, rotation, scale));
        
        current_life_time -= Time.deltaTime;

        return current_life_time > 0;
    }

    public Mesh ParticleMesh => particle_mesh;

    public Matrix4 TransformationMatrix => transformation_matrix;

    public Vector4 Color => color;
}