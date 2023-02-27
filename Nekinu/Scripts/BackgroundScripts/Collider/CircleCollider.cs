using NekinuSoft;

namespace Nekinu.Collider
{
    public class CircleCollider : Collider
    {
        private float radius;


        public void PositionalCorrection(Manifold manifold)
        {
            const float per = 0.2f;

            Vector3 correction = Math.Max(manifold.ColliderPenetration - 0.01f, 0f) / (manifold.A.InvMass + manifold.B.InvMass) * per * manifold.Normal;
            manifold.A.Parent.Transform.position -= manifold.A.InvMass * correction;
            manifold.B.Parent.Transform.position += manifold.B.InvMass * correction;
        }
        
        public bool CircleVsCircleCollision(Manifold manifold)
        {
            CircleCollider A = (CircleCollider) manifold.A;
            CircleCollider B = (CircleCollider) manifold.B;

            Vector3 n = B.Parent.Transform.position - A.Parent.Transform.position;
            
            float r = A.radius + B.radius;
            r *= r;

            if (n.Length() > r)
            {
                return false;
            }

            float d = n.Length();

            if (d != 0)
            {
                manifold.ColliderPenetration = r - d; 
            }

            return r < Math.Pow(A.Parent.Transform.position.x + B.Parent.Transform.position.x, 2) + Math.Pow(A.Parent.Transform.position.y + B.Parent.Transform.position.y, 2);
        }

        public float Radius => radius;
    }
}