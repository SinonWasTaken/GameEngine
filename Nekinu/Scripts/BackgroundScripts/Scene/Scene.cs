namespace NekinuSoft.Scene_Manager
{
    public class Scene
    {
        //The name of the scene
        private string scene_name;

        //All entities in a scene
        private List<Entity> scene_entities;

        //Scene constructor
        public Scene(string sceneName)
        {
            scene_name = sceneName;
            scene_entities = new List<Entity>();
        }

        //Called when a scene is loaded
        public void Awake()
        {
            for (int i = 0; i < scene_entities.Count; i++)
            {
                scene_entities[i].Awake();
            }
        }

        //Updates a scene
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
        
        //Adds an entity to the scene
        public void AddEntity(Entity entity)
        {
            scene_entities.Add(entity);
        }

        //Removes an entity from the scene
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

        //Gets an entity from the scene, by its name
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
        
        //Don't know why this method exists
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

        //Called when the scene is unloaded
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