using NekinuSoft;

namespace Nekinu.Collider
{
    public class Collider : Component
    {
        private float mass;
        private float invMass;
        private float drag = 1;
        private float restitution = 5;

        private bool isTrigger;
        
        private Vector3 velocity;
        
        public enum ColliderPhysics
        {
            Impulse
        }

        public float Mass
        {
            get => mass;
            set
            {
                mass = value;
                invMass = 1 / mass;
            }
            
        }

        public float InvMass => invMass;
    }
}