using Nekinu;
using NekinuSoft;

public class Particle_System : Component
{
    //contains the list of particles alive in this system
    private List<Particle> particles;

    //The default life of each particle
    [SerializedProperty] private float particle_Duration;
    
    //Does the system loop when all particles are dead
    [SerializedProperty] private bool loop;
    //Plays when the particle system is spawned into the world
    [SerializedProperty] private bool play_on_awake;
    
    //The amount of particles that should be emitted on play
    [SerializedProperty] private int particles_to_emit;

    //The speed of the particles
    [SerializedProperty] private float speed;

    //The gravity modifier of the particle
    [SerializedProperty] private float gravity_scale;

    //The colors of the particles
    [SerializedProperty] private Vector4 color;

    //Has the system started emitting particles
    private bool started = false;

    public Particle_System()
    {
        particles = new List<Particle>();
        particle_Duration = 5f;
        loop = false;
        play_on_awake = true;
        particles_to_emit = 20;
        speed = 1f;
        gravity_scale = 1;
        color = new Vector4(1, 1, 1, 1);
    }

    public Particle_System(float particleDuration, bool loop, bool playOnAwake, int particlesToEmit, float speed, float gravityScale, Vector4 color)
    {
        particles = new List<Particle>();
        
        particle_Duration = particleDuration;
        this.loop = loop;
        play_on_awake = playOnAwake;
        particles_to_emit = particlesToEmit;
        this.speed = speed;
        gravity_scale = gravityScale;
        this.color = color;
    }

    //Starts the particle system
    public void StartSystem()
    {
        if (!started)
        {
            started = true;

            //Creates a random number generator
            Random random = new Random();

            //Creates the specified amount of particles
            for (int i = 0; i < particles_to_emit; i++)
            {
                particles.Add(new Particle(ResourceGetter.Get_Resource_File_Of_Type_String("Particle", ".obj"), particle_Duration,  speed, new Vector3(random.Next(-50, 51), random.Next(30, 70), random.Next(-50, 51)), Parent.Transform.position, Vector3.zero, Vector3.one, gravity_scale, color));
            }

            started = false;
        }
    }

    public override void Awake()
    {
        //Called when the system is added to the world
        base.Awake();
        if (play_on_awake)
        {
            StartSystem();
        }
    }
    
    public override void Update()
    {
        //Updates each particle
        for (int i = 0; i < particles.Count; i++)
        {
            //If the life of the particle is 0
            if (!particles[i].Update())
            {
                //Then remove particle from the list
                particles.Remove(particles[i]);
            }
        }

        if (particles.Count == 0)
        {
            if (loop)
            {
                StartSystem();
            }
        }
    }

    public List<Particle> Particles => particles;
}