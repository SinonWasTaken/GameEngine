using NekinuSoft.Scene_Manager;

namespace NekinuSoft
{
    //The base class for components. Components can be added to entity to provide functionality
    public class Component
    {
        //If the component is active. Will prevent the component from being used, if it isnt active
        private bool isActive = true;

        //The entity the component is attached to
        public Entity Parent { get; private set; }

        //Called when an objec is spawned into the world
        public virtual void Awake()
        {
        }

        //Same method as Awake, but called after.
        public virtual void Start()
        {
        }

        //Called each from
        public virtual void Update() { }

        //Called when the component is enabled after being disabled
        public virtual void OnEnabled()
        {
        }

        //Called when the component is disabled
        public virtual void OnDisabled() { }

        //Called when the component is removed, of the parent is destoryed
        public virtual void OnDestroy() { }
        
        //Determines if the component is active or not
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

        //Sets the parent of the component
        public virtual void Set_Parent(Entity entity)
        {
            Parent = entity;
        }

        //Removes the parent from the scene
        public void Destroy()
        {
            SceneManager.RemoveEntityFromScene(Parent);
        }
        
        //A static way to remove entities from the scne
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