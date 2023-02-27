using NekinuSoft;
using OpenTK.Mathematics;

namespace Nekinu.Animation
{
    public class AnimationModel : Component
    {
        private VAO model;
        private Texture texture;

        private Joint rootJoint;
        private int jointCount;

        private Animator anim;

        public AnimationModel(VAO model, Texture texture, Joint rootJoint, int jointCount)
        {
            this.model = model;
            this.texture = texture;
            this.rootJoint = rootJoint;
            this.jointCount = jointCount;
        }
        
        public void CleanUp()
        {
            model.CleanUp();
        }

        public void PerformAnimation()
        {
            anim.Update();
        }

        public Matrix4[] getJointTransformation()
        {
            Matrix4[] jointMatrix = new Matrix4[jointCount];
            AddJointToArray(rootJoint, jointMatrix);
            return jointMatrix;
        }

        private void AddJointToArray(Joint joint, Matrix4[] jointMatrix)
        {
            jointMatrix[joint.Index] = joint.AnimatedTransform;

            foreach (Joint child in joint.Children)
            {
                AddJointToArray(child, jointMatrix);
            }
        }
    }
}