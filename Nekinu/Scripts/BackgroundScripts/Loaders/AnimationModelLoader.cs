using NekinuSoft;

namespace Nekinu.Animation.Loaders
{
    public class AnimationModelLoader
    {
        /*public AnimationModel LoadModel(string modelFile)
        {
            AnimationModelData modelData = ColladaLoader.LoadColladaModel(modelFile);
            //VAO model =

            JointData data = modelData.getJointData();
            Joint head = createJoints(data.Head);
        }*/

        /*private Joint createJoints(JointData head)
        {
            Joint joint = new Joint(head.Index, head.NameID, head.LocalBindTransform);
            foreach (JointData child in head.Children)
            {
                joint.AddJoint(createJoints(child));
            }

            return joint;
        }*/

        /*private static VAO createVao(Mesh data)
        {
            VAO vao = new VAO();
            vao.createVAO();
            vao.Bind();
            vao.bindIndicesBuffer(data.vao.get());
            vao.storeData(0, 3, data.vao.get());
            vao.storeData(1, 2, data.vao.GetTextureCoords());
            vao.storeData(2, 3, data.vao.GetNormals());
            vao.storeIntData(3,3, data.vao.getJointIds());
            vao.storeData(4, 3, data.vao.GetJointWeights());
            vao.Unbind();
            return vao;
        }*/
    }
}