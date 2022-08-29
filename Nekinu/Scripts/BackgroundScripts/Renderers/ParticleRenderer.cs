using NekinuSoft;
using NekinuSoft.Renderer;
using NekinuSoft.Scene_Manager;
using OpenTK.Graphics.OpenGL;

public class ParticleRenderer : IRenderer
{
    private Particle_Shader shader;
    
    public ParticleRenderer()
    {
        shader = new Particle_Shader(ResourceGetter.Get_Resource_File_Of_Type_String("Particle_Vertex", ".txt"), ResourceGetter.Get_Resource_File_Of_Type_String("Particle_Fragment", ".txt"));
    }
    
    public override void Render(Camera camera)
    {
        List<Entity> particle_systems = SortParticleSystems();

        //bind shader, load projection, and unbind
        
        for (int i = 0; i < particle_systems.Count; i++)
        {
            shader.Bind();
            Particle_System system = particle_systems[i].GetComponent<Particle_System>();

            if (system.Particles != null && system.Particles.Count > 0)
            {
                Particle particle = system.Particles[0];
                GL.BindVertexArray(particle.ParticleMesh.vao.vao);

                GL.EnableVertexAttribArray(0);
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                GL.DepthMask(false);

                shader.loadView(camera.View);
                shader.loadProjection(camera.Projection);

                for (int j = 0; j < system.Particles.Count; j++)
                {
                    shader.loadTransformation(system.Particles[j].TransformationMatrix);
                    shader.loadColor(particle.Color);

                    GL.DrawElements(BeginMode.Triangles, particle.ParticleMesh.vertex_count,
                        DrawElementsType.UnsignedInt, IntPtr.Zero);

                    //Mental note. I dont want to worry about rotating a flat plane to face the camera at all times, so it will initially be a square. If i wish to increase performance, follow thinmatrix particle system tut closer
                }

                GL.DepthMask(true);
                GL.Disable(EnableCap.Blend);
                GL.DisableVertexAttribArray(0);
                GL.BindVertexArray(0);
            }

            shader.Unbind();
        }
    }

    private List<Entity> SortParticleSystems()
    {
        List<Entity> entities = new List<Entity>();

        for (int i = 0; i < SceneManager.GetSceneEntities().Count; i++)
        {
            Particle_System system = SceneManager.GetSceneEntities()[i].GetComponent<Particle_System>();

            if (system != null)
            {
                entities.Add(SceneManager.GetSceneEntities()[i]);
            }
        }

        return entities;
    }
}