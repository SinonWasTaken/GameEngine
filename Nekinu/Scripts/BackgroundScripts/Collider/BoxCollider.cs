using NekinuSoft;

namespace Nekinu.Collider;

public class BoxCollider : Collider
{
    private Vector3 min;
    private Vector3 max;

    public Vector3 Min => min;
    public Vector3 Max => max;

    public bool BoxVsBoxCollision(Manifold manifold)
    {
        Vector3 n = manifold.B.Parent.Transform.position - manifold.A.Parent.Transform.position;

        BoxCollider A = (BoxCollider) manifold.A;
        BoxCollider B = (BoxCollider) manifold.B;

        float a_extent = (A.max.x - A.min.x) / 2;
        float b_extent = (B.max.x - B.min.x) / 2;

        float x_overlap = a_extent + b_extent - Math.Abs(n.x);

        if (x_overlap > 0)
        {
            a_extent = (A.max.y - A.min.y) / 2;
            b_extent = (B.max.y - B.min.y) / 2;
        }

        return true;
    }
}