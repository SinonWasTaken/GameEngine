using Nekinu;
using NekinuSoft;

public class Particle_System : Component
{
    private List<Particle> particles;

    [SerializedProperty] private float particle_Duration;
    
    [SerializedProperty] private bool loop;
    [SerializedProperty] private bool play_on_awake;
    
    [SerializedProperty] private int particles_to_emit;

    [SerializedProperty] private float speed;

    [SerializedProperty] private float gravity_scale;

    [SerializedProperty] private Vector4 color;

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

    public void StartSystem()
    {
        if (!started)
        {
            started = true;

            Random random = new Random();

            for (int i = 0; i < particles_to_emit; i++)
            {
                particles.Add(new Particle(ResourceGetter.Get_Resource_File_Of_Type_String("Particle", ".obj"), particle_Duration,  speed, new Vector3(random.Next(-50, 51), random.Next(30, 70), random.Next(-50, 51)), Parent.Transform.position, Vector3.zero, Vector3.one, gravity_scale, color));
            }

            started = false;
        }
    }

    public override void Awake()
    {
        base.Awake();
        if (play_on_awake)
        {
            StartSystem();
        }
    }

    public override void Update()
    {
        for (int i = 0; i < particles.Count; i++)
        {
            if (!particles[i].Update())
            {
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