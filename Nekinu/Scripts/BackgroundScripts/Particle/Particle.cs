using NekinuSoft;
using NekinuSoft.MeshLoader;
using OpenTK.Mathematics;
using Vector3 = NekinuSoft.Vector3;
using Vector4 = NekinuSoft.Vector4;

public class Particle
{
    //The particle mesh
    private Mesh particle_mesh;

    //The total life of the particle
    private float total_life_time;
    //The current life of the particle
    private float current_life_time;
    //The speed the particle moves
    private float speed;
    
    //The velocity of the particle
    private Vector3 velocity;
    
    //The position of the particle
    private Vector3 position;
    //The rotation of the particle
    private Vector3 rotation;
    //The scale of the particle
    private Vector3 scale;

    //The gravity multiplier of the particle
    private float gravity_scale;

    //The color of the particle
    private Vector4 color;
    
    //The matrix of the particle
    public Matrix4 transformation_matrix;

    //Default constructor
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

    //Updates the particle
    public bool Update()
    {
        //updates the velocity
        velocity += new Vector3(0, -50f * speed * gravity_scale * Time.deltaTime, 0);
        
        //updates the velocity by the delta time
        Vector3 change = new Vector3(velocity.x, velocity.y, velocity.z);
        change *= Time.deltaTime;
        
        //updates the position based on the velocity
        position += change;

        //updates the transformation matrix
        transformation_matrix = Matrix4x4.cameraTransformationMatrix(new Transform(position, rotation, scale));
        
        //decreases the life of the particle
        current_life_time -= Time.deltaTime;

        //Destroys the particle if the currentLife is less than 0
        return current_life_time > 0;
    }

    public Mesh ParticleMesh => particle_mesh;

    public Matrix4 TransformationMatrix => transformation_matrix;

    public Vector4 Color => color;
}