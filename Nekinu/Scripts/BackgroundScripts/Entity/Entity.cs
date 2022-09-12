using OpenTK.Mathematics;

namespace NekinuSoft
{
    public class Entity
    {
        //The entity name
        private string entity_name;

        //The tag of the entity
        private string entity_tag;

        //The parent entity
        private Entity parent;

        //The world transform of the entity
        private Transform transform;

        //List of components
        private List<Component> _components;

        //List of children
        private List<Entity> children;

        //Default constructor
        public Entity(){}
        
        //Default constructor
        public Entity(string name)
        {
            initEntity(name, new Transform());
        }

        //Default constructor
        public Entity(string name, Transform transform)
        {
            initEntity(name, transform);
        }

        //Default method that sets up the entity
        private void initEntity(string name, Transform transform)
        {
            IsActive = true;
            entity_name = name;

            entity_tag = "Default";

            _components = new List<Component>();
            children = new List<Entity>();

            this.transform = transform;   
        }

        //Adds a child entity to the list
        public void AddChild(Entity entity)
        {
            entity.SetParent(this);
            children.Add(entity);
        }

        //Removes a child entity from the list
        public void RemoveChild(Entity entity)
        {
            entity.SetParent(null);
            children.Remove(entity);
        }
        
        //Finds a child entity from this entity
        public Entity Find(string name)
        {
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].entity_name == name)
                {
                    return children[i];
                }
                else
                {
                    Entity return_entity = children[i].Find(name);

                    if (return_entity != null)
                    {
                        return return_entity;
                    }
                }
            }

            return null;
        }

        //Sets the parent entity
        public void SetParent(Entity entity)
        {
            parent = entity;
        }
        
        //Adds a component to the entity
        public void AddComponent(Component component)
        {
            component.Set_Parent(this);
            _components.Add(component);
        }
        //Adds a default component of type T 
        public void AddComponent<T>() where T : Component
        {
            Type type = typeof(T);

            Component component = Activator.CreateInstance(type) as Component;
            component.Set_Parent(this);
            _components.Add(component);
        }

        //Gets a component of type T from the entity
        public T GetComponent<T>() where T : Component
        {
            foreach (Component component in _components)
            {
                if (component is T)
                {
                    return (T) component;
                }
            }

            return null;
        }

        //Called every frame
        public void Update()
        {
            for (int i = 0; i < _components.Count; i++)
            {
                //Only updates a component if it is active
                if (_components[i].IsActive)
                {
                    _components[i].Update();
                }
            }
        }

        //Called when the entity is added to the scene, or when the scene starts
        public void Awake()
        {
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Awake();
                _components[i].Start();
            }
        }

        //Called when the entity is removed from the scene
        public void Destroy()
        {
            //If this entity has a parent, remove it from the parent
            if (parent != null)
            {
                parent.RemoveChild(this);
            }
            
            //Destroy any components attached
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].OnDestroy();
            }

            //Destroy any child entitys attached
            for (int i = 0; i < children.Count; i++)
            {
                children[i].Destroy();
            }
        }
        
        public bool IsActive { get; set; }
        
        public string EntityName => entity_name;

        public Entity Parent => parent;

        public List<Component> Components => _components;
        public List<Entity> Children => children;
        
        //Gets the transformation matrix of the entity
        public Matrix4 TransformationMatrix => Matrix4x4.entityTransformationMatrix(parent, transform);

        public Transform Transform
        {
            get => transform;
            set => transform = value;
        }
    }
}