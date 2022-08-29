using NekinuSoft.Scene_Manager;

namespace NekinuSoft
{
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
        
        private void LoadCameraViewMatrix(Camera camera)
        {
            UniformMatrix4(location_view, camera.View);
        }
        private void LoadCameraProjectionMatrix(Camera camera)
        {
            UniformMatrix4(location_projection, camera.Projection);
        }

        public void Load_Camera(Camera camera)
        {
            LoadCameraProjectionMatrix(camera);
            LoadCameraViewMatrix(camera);
        }

        public virtual void LoadLights(Entity render_object) { }
        
        public virtual void Load_Entity_Variables(Entity entity)
        {
            UniformMatrix4(location_transformation, entity.TransformationMatrix);
        }
    }
}