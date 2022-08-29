using OpenTK.Mathematics;

namespace NekinuSoft
{
    public class Entity
    {
        private string entity_name;

        private string entity_tag;

        private Entity parent;

        private Transform transform;

        private List<Component> _components;

        private List<Entity> children;

        public Entity(){}
        
        public Entity(string name)
        {
            initEntity(name, new Transform());
        }

        public Entity(string name, Transform transform)
        {
            initEntity(name, transform);
        }

        private void initEntity(string name, Transform transform)
        {
            IsActive = true;
            entity_name = name;

            entity_tag = "Default";

            _components = new List<Component>();
            children = new List<Entity>();

            this.transform = transform;   
        }

        public void AddChild(Entity entity)
        {
            entity.SetParent(this);
            children.Add(entity);
        }

        public void RemoveChild(Entity entity)
        {
            entity.SetParent(null);
            children.Remove(entity);
        }
        
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

        public void SetParent(Entity entity)
        {
            parent = entity;
        }
        
        public void AddComponent(Component component)
        {
            component.Set_Parent(this);
            _components.Add(component);
        }
        public void AddComponent<T>() where T : Component
        {
            Type type = typeof(T);

            Component component = Activator.CreateInstance(type) as Component;
            component.Set_Parent(this);
            _components.Add(component);
        }

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

        public void Update()
        {
            for (int i = 0; i < _components.Count; i++)
            {
                if (_components[i].IsActive)
                {
                    _components[i].Update();
                }
            }
        }

        public void Awake()
        {
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Awake();
                _components[i].Start();
            }
        }

        public void Destroy()
        {
            if (parent != null)
            {
                parent.RemoveChild(this);
            }
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].OnDestroy();
            }

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
        
        public Matrix4 TransformationMatrix => Matrix4x4.entityTransformationMatrix(parent, transform);
        
        public string Tag
        {
            get => entity_tag;
            set => entity_tag = value;
        }

        public Transform Transform
        {
            get => transform;
            set => transform = value;
        }
    }
}