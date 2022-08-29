using Nekinu;
using OpenTK.Mathematics;

namespace NekinuSoft
{
    public class Camera : Component, EditorUpdate
    {
        public enum Camera_Projection
        {
            perspective,
            orthographic
        }

        public enum Background_Type
        {
            Image,
            Solid_Color
        }

        public static Camera Main_Camera { get; set; }

        public Vector4 skyColor;
        
        [SerializedProperty] private float fov;
        [SerializedProperty] private float near;
        [SerializedProperty] private float far;

        private bool is_main;

        [SerializedProperty] private float ortho_size = 10;

        public Camera() { }

        public override void Awake()
        {
            base.Awake();
            fov = 90;
            near = 0.01f;
            far = 1000f;

            Camera_projection = Camera_Projection.perspective;
            skyColor = new Vector4(0.640625f, 0.85882352941176470588235294117647f, 0.90980392156862745098039215686275f, 1f);
        }

        public void EditorAwake()
        {
            Awake();
        }

        public Matrix4 set_projection()
        {
            if (Camera_projection == Camera_Projection.perspective)
            {
                return Matrix4.CreatePerspectiveFieldOfView(fov * Math.Math.ToRadians, WindowSize.AspectRatio, near, far);
            }
            else
            {
                return Matrix4.CreateOrthographic(ortho_size, ortho_size, near, far);
            }
        }

        public override void Set_Parent(Entity entity)
        {
            base.Set_Parent(entity);

            Parent.Tag = is_main ? "Main_Camera" : Parent.Tag;
        }
        
        public Camera_Projection Camera_projection
        {
            get;
            set;
        }
        
        public Matrix4 Projection
        {
            get => set_projection();
        }

        public Matrix4 View
        {
            get => Matrix4x4.cameraTransformationMatrix(Parent.Parent != null ? Parent.Parent : null, Parent.Transform);
        }
        
        public float Ortho_Size
        {
            get => ortho_size;
            set
            {
                float new_value = value;

                if (new_value < 10)
                {
                    new_value = 10;
                }

                ortho_size = new_value;

                set_projection();
            }
        }
        public float Fov
        {
            get => fov;
            set
            {
                fov = value;
                
                set_projection();
            }
        }
        public float Near
        {
            get => near;
            set
            {
                near = value;
                
                set_projection();
            }
        }
        public float Far
        {
            get => far;
            set
            {
                far = value;
                
                set_projection();
            }
        }
    }
}