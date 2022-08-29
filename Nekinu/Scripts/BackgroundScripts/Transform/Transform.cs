using Newtonsoft.Json;
using OpenTK;

namespace NekinuSoft
{
    public class Transform
    {
        [JsonProperty] public Vector3 position { get; set; }

        [JsonProperty] private Vector3 rot;

        [JsonIgnore]
        public Vector3 rotation
        {
            get => rot;
            set => rot = value;
        }

        [JsonProperty] public Vector3 scale { get; set; }

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

        private void initTransform(Vector3 p, Vector3 r, Vector3 s)
        {
            position = p;
            rotation = r;
            scale = s;
        }

        public Vector3 forward
        {
            get
            {
                return Quaternion.FromEulerAngles(new Vector3(rotation.x, -rotation.y, -rotation.z)) * Vector3.forward;
            }
        }

        public Vector3 up
        {
            get { return Quaternion.FromEulerAngles(new Vector3(rotation.x, -rotation.y, -rotation.z)) * Vector3.up; }
        }

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