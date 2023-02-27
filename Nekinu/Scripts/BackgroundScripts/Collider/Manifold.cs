using NekinuSoft;

namespace Nekinu.Collider;

public class Manifold
{
    private Collider a;
    private Collider b;
    private float colliderPenetration;
    private Vector3 normal;

    public Collider A => a;

    public Collider B => b;

    public float ColliderPenetration
    {
        get => colliderPenetration;
        set => colliderPenetration = value;
    }

    public Vector3 Normal
    {
        get => normal;
        set => normal = value ?? throw new ArgumentNullException(nameof(value));
    }
}