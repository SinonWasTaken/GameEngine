using NekinuSoft.Scene_Manager;

namespace NekinuSoft
{
    public class Component
    {
        private bool isActive = true;

        public Entity Parent { get; private set; }

        public virtual void Awake()
        {
        }

        public virtual void Start()
        {
        }

        public virtual void Update() { }

        public virtual void OnEnabled()
        {
        }

        public virtual void OnDisabled() { }

        public virtual void OnDestroy() { }
        
        public void Set_Active(bool active = true)
        {
            if (!isActive)
            {
                if (active)
                {
                    OnEnabled();
                }
            }
            else
            {
                if (!active)
                {
                    OnDisabled();
                }
            }

            isActive = active;
        }

        public virtual void Set_Parent(Entity entity)
        {
            Parent = entity;
        }

        public void Destroy()
        {
            SceneManager.RemoveEntityFromScene(Parent);
        }
        
        public static void Destroy(Entity entity)
        {
            SceneManager.RemoveEntityFromScene(entity);
        }

        public bool IsActive
        {
            get => isActive;
            set => isActive = value;
        }
    }
}