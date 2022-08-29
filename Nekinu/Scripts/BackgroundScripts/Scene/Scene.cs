namespace NekinuSoft.Scene_Manager
{
    public class Scene
    {
        private string scene_name;

        private List<Entity> scene_entities;

        public Scene(string sceneName)
        {
            scene_name = sceneName;
            scene_entities = new List<Entity>();
        }

        public void Awake()
        {
            for (int i = 0; i < scene_entities.Count; i++)
            {
                scene_entities[i].Awake();
            }
        }

        public void Update()
        {
            for (int i = 0; i < scene_entities.Count; i++)
            {
                if (scene_entities[i].IsActive)
                {
                    scene_entities[i].Update();
                }
            }
        }
        
        public void AddEntity(Entity entity)
        {
            scene_entities.Add(entity);
        }

        public Entity RemoveEntity(Entity entity)
        {
            bool removed = scene_entities.Remove(entity); 
            if (!removed)
            {
                if (entity.Parent != null)
                {
                    entity.Parent.RemoveChild(entity);
                }
            }

            return entity;
        }

        public Entity GetEntity(string name)
        {
            for (int i = 0; i < scene_entities.Count; i++)
            {
                if (scene_entities[i].IsActive)
                {
                    if (scene_entities[i].EntityName == name)
                    {
                        return scene_entities[i];
                    }
                }
            }

            return null;
        }
        
        public Entity GetEntity(Entity entity)
        {
            for (int i = 0; i < scene_entities.Count; i++)
            {
                if (scene_entities[i].IsActive)
                {
                    if (scene_entities[i] == entity)
                    {
                        return scene_entities[i];
                    }
                }
            }

            return null;
        }

        public void CloseScene()
        {
            for (int i = 0; i < scene_entities.Count; i++)
            {
                scene_entities[i].Destroy();
            }
        }
        
        public string SceneName => scene_name;
        public List<Entity> SceneEntities => scene_entities;
    }
}