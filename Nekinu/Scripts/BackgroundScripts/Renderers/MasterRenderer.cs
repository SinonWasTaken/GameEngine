using NekinuSoft.Renderer;
using NekinuSoft.Scene_Manager;
using OpenTK.Graphics.ES30;

namespace NekinuSoft
{
    //The master renderer. Contains a list of all renderers, and calls each render method
    public static class MasterRenderer
    {
        private static List<IRenderer> renderers;

        private static ParticleRenderer particle_renderer;
        private static UI_Renderer ui_renderer;
        
        public static void InitMasterRenderer(params IRenderer[] render)
        {
            renderers = new List<IRenderer>();
            renderers.AddRange(render);

            //Adds the ui renderer and particles renderer last, so they are rendered on top
            ui_renderer = new UI_Renderer();
            particle_renderer = new ParticleRenderer();
        }

        //Adds a renderer to the list
        public static void AddRenderer(IRenderer renderer)
        {
            renderers.Add(renderer);
        }
        
        public static void Render()
        {
            //Gets the camera to render from
            Entity camera = getCamera();
            
            //If there is a camera to render from
            if (camera != null)
            {
                //Get the camera component
                Camera cam = camera.GetComponent<Camera>();
                //Set the background color to the camera skyColor
                GL.ClearColor(cam.skyColor.x, cam.skyColor.y, cam.skyColor.z, 1);
                
                //Loop through each render
                for (int i = 0; i < renderers.Count; i++)
                {
                    try
                    {
                        //and call the render method
                        renderers[i].Render();
                        renderers[i].Render(cam);
                    }
                    catch (Exception e)
                    {
                        //If there is any error when rendering, create a crash report, and exit the program 
                        Crash_Report.generate_crash_report($"Error rendering! :{e}");
                        Environment.Exit(-224);
                    }
                }
                
                //Renders any particles
                particle_renderer.Render(cam);
                //Renders an ui object
                ui_renderer.Render(cam);
            }
            else
            {
                //Set the back ground color to black
                GL.ClearColor(0,0,0,1);
                //Tells the user that no camera exists to render from
                Debug.WriteLog("No camera to render from");
            }
        }
        
        //Gets a camera from the scene. Meant to get the main camera, not just any camera
        private static Entity getCamera()
        {
            Entity camera = SceneManager.GetEntityFromScene("Camera");

            return camera;
        }

        public static void Resize(int x, int y)
        {
            //Not used currently 
        }
        
        //Called when the program ends
        public static void CloseRenderer()
        {
            //Calls the close method on every renderer
            for (int i = 0; i < renderers.Count; i++)
            {
                renderers[i].Close();
            }
        }
    }
}