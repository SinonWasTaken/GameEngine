using Nekinu;
using OpenTK.Mathematics;

namespace NekinuSoft
{
    public class Camera : Component, EditorUpdate
    {
        //Enum for the camera perspective
        public enum Camera_Projection
        {
            perspective,
            orthographic
        }

        //Enum that determines if the back is a simple color, or an image
        public enum Background_Type
        {
            Image,
            Solid_Color
        }

        //a public variable that contains information on the main camera in a scene
        public static Camera Main_Camera { get; set; }

        //The scy color, if the background isnt an image
        public Vector4 skyColor;
        
        //The field of view of the camera
        [SerializedProperty] private float fov;
        //How close to the camera an object is renderered
        [SerializedProperty] private float near;
        //How far a camera can see
        [SerializedProperty] private float far;

        //The size of the camera, if the camera is an orthographic camera
        [SerializedProperty] private float ortho_size = 10;

        //Default constructor. Used for saving and loading objects
        public Camera() { }

        //Called when an object is brought into the scene, or when the scene starts
        public override void Awake()
        {
            base.Awake();
            //Sets the deault values of the camera
            fov = 90;
            near = 0.01f;
            far = 1000f;

            //Sets the projection to a perspective projection, I.E a normal camera
            Camera_projection = Camera_Projection.perspective;
            //Sets the background color
            skyColor = new Vector4(0.640625f, 0.85882352941176470588235294117647f, 0.90980392156862745098039215686275f, 1f);
        }

        //Only called in the editor
        public void EditorAwake()
        {
            Awake();
        }

        //Returns the projection of the camera
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

        //Sets the parent of the camera
        public override void Set_Parent(Entity entity)
        {
            base.Set_Parent(entity);
        }
        
        //Getter and setter method for the variables above. Allows user to get values, and set them
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