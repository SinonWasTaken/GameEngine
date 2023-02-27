using OpenTK.Mathematics;

namespace Nekinu.Animation;

public class Joint
{
    private int index;
    private string name;
    private List<Joint> children;

    private Matrix4 animatedTransform;

    private Matrix4 localBindTransform;

    private Matrix4 inverseBindTransform;

    public Joint(int index, string name, Matrix4 localBindTransform)
    {
        this.index = index;
        this.name = name;
        this.localBindTransform = localBindTransform;

        children = new List<Joint>();
    }

    public void AddJoint(Joint joint)
    {
        children.Add(joint);
    }

    public Matrix4 AnimatedTransform
    {
        get => animatedTransform;
        set => animatedTransform = value;
    }

    public Matrix4 InverseBindTransform => inverseBindTransform;

    public void calcInverseBindTransform(Matrix4 parentBind)
    {
        Matrix4 bind = Matrix4.Mult(parentBind, localBindTransform);

        inverseBindTransform = Matrix4.Invert(bind);

        foreach (Joint joint in children)
        {
            joint.calcInverseBindTransform(bind);
        }
    }

    public int Index => index;

    public string Name => name;

    public List<Joint> Children => children;

    public Matrix4 LocalBindTransform => localBindTransform;
}