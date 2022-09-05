using Newtonsoft.Json;
using OpenTK;

namespace NekinuSoft
{
    public class Transform
    {
        //A 3 valued class. x,y,z. Stores the position of the object in the world
        [JsonProperty] public Vector3 position { get; set; }

        //A 3 valued class. x,y,z. Stores the rotation of the object in the world
        [JsonProperty] private Vector3 rot;

        [JsonIgnore]
        public Vector3 rotation
        {
            get => rot;
            set => rot = value;
        }

        //A 3 valued class. x,y,z. Stores the scale of the object in the world
        [JsonProperty] public Vector3 scale { get; set; }

        //Constructors
        public Transform()
        {
            initTransform(Vector3.zero, Vector3.zero, Vector3.one);
        }
        public Transform(Vector3 position)
        {
            initTransform(position, Vector3.zero, Vector3.one);
        }
        public Transform(Vector3 position, Vector3 rotation)
        {
            initTransform(position, rotation, Vector3.one);
        }
        public Transform(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            initTransform(position, rotation, scale);
        }
        
        //A method used to set information for the transform. Reduced duplicate code
        private void initTransform(Vector3 p, Vector3 r, Vector3 s)
        {
            position = p;
            rotation = r;
            scale = s;
        }

        //Allows an object to move forward, even if the object is rotated
        public Vector3 forward
        {
            get
            {
                return Quaternion.FromEulerAngles(new Vector3(rotation.x, -rotation.y, -rotation.z)) * Vector3.forward;
            }
        }

        //Allows an object to move up, even if the object is rotated
        public Vector3 up
        {
            get { return Quaternion.FromEulerAngles(new Vector3(rotation.x, -rotation.y, -rotation.z)) * Vector3.up; }
        }

        //Allows an object to move right, even if the object is rotated
        public Vector3 right
        {
            get
            {
                return Quaternion.FromEulerAngles(new Vector3(rotation.x, -rotation.y, -rotation.z)) * Vector3.right;
            }
        }

        public override string ToString()
        {
            return "Transform: Position " + position.ToString() + " Rotation " + rotation.ToString() + " Scale " +
                   scale.ToString();
        }
    }
}