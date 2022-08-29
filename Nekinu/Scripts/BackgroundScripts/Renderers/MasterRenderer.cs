using NekinuSoft.Renderer;
using NekinuSoft.Scene_Manager;
using OpenTK.Graphics.ES30;

namespace NekinuSoft
{
    public static class MasterRenderer
    {
        private static List<IRenderer> renderers;

        private static ParticleRenderer particle_renderer;
        private static UI_Renderer ui_renderer;
        
        public static void InitMasterRenderer(params IRenderer[] render)
        {
            renderers = new List<IRenderer>();
            renderers.AddRange(render);

            ui_renderer = new UI_Renderer();
            particle_renderer = new ParticleRenderer();
        }

        public static void AddRenderer(IRenderer renderer)
        {
            renderers.Add(renderer);
        }
        
        public static void Render()
        {
            Entity camera = getCamera();
            if (camera != null)
            {
                Camera cam = camera.GetComponent<Camera>();
                GL.ClearColor(cam.skyColor.x, cam.skyColor.y, cam.skyColor.z, 1);
                for (int i = 0; i < renderers.Count; i++)
                {
                    try
                    {
                        renderers[i].Render();
                        renderers[i].Render(cam);
                    }
                    catch (Exception e)
                    {
                        Crash_Report.generate_crash_report($"Error rendering! :{e}");
                        Environment.Exit(-224);
                    }
                }
                
                particle_renderer.Render(cam);
                ui_renderer.Render(cam);
            }
            else
            {
                GL.ClearColor(0,0,0,1);
                Debug.WriteLog("No camera to render from");
            }
        }
        
        private static Entity getCamera()
        {
            Entity camera = SceneManager.GetEntityFromScene("Camera");

            return camera;
        }

        public static void Resize(int x, int y)
        {
            
        }
        
        public static void CloseRenderer()
        {
            for (int i = 0; i < renderers.Count; i++)
            {
                renderers[i].Close();
            }
        }
    }
}