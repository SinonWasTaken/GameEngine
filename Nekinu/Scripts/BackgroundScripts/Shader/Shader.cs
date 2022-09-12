using NekinuSoft.Scene_Manager;

namespace NekinuSoft
{
    //A default shader for objects
    public class Shader : ShaderProgram
    {
        private int location_transformation;
        private int location_projection;
        private int location_view;

        public Shader(string vertex, string fragment) : base(vertex, fragment)
        {
        }

        public override void BindAttributes()
        {
            BindAttribute(0, "position");
            BindAttribute(1, "texture");
            BindAttribute(2, "normal");
        }

        public override void GetAllUniformLocations()
        {
            location_transformation = GetUniformLocation("transformation_matrix");
            location_projection = GetUniformLocation("projection_matrix");
            location_view = GetUniformLocation("view_matrix");
        }

        protected List<Light> GetSceneLights()
        {
            List<Light> lights = new List<Light>();

            for (int i = 0; i < SceneManager.GetSceneEntities().Count; i++)
            {
                Light light = SceneManager.GetSceneEntities()[i].GetComponent<Light>();
                if (light != null)
                {
                    lights.Add(light);
                }
            }

            return lights;
        }
        
        //Loads the cameras view matrix
        private void LoadCameraViewMatrix(Camera camera)
        {
            UniformMatrix4(location_view, camera.View);
        }
        //Loads the cameras projection matrix
        private void LoadCameraProjectionMatrix(Camera camera)
        {
            UniformMatrix4(location_projection, camera.Projection);
        }

        //Loads the camera
        public void Load_Camera(Camera camera)
        {
            LoadCameraProjectionMatrix(camera);
            LoadCameraViewMatrix(camera);
        }

        //Loads a light into the shader
        public virtual void LoadLights(Entity render_object) { }
    }
}