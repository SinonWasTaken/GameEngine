namespace NekinuSoft.Scene_Manager
{
    public static class SceneManager
    {
        public delegate void SceneLoad(Scene scene);
        public static event SceneLoad OnSceneLoad;

        public delegate void EntityAdd(Entity entity);
        public static event EntityAdd EntityAdded;
        public static event EntityAdd EntityRemoved;
        
        private static List<Scene> scenes;
        
        private static Scene loaded_scene;
        
        public static void InitSceneManager()
        {
            scenes = new List<Scene>();
        }

        public static void UpdateScene()
        {
            loaded_scene?.Update();
        }

        public static void AddScene(params Scene[] scene_loading)
        {
            scenes.AddRange(scene_loading);
        }
        public static void RemoveScene(Scene scene)
        {
            scenes.Remove(scene);
        }
        
        public static void LoadScene(int id)
        {
            try
            {
                if (loaded_scene != null)
                {
                    loaded_scene.CloseScene();
                    
                    /*if(OnSceneUnload != null)
                        OnSceneUnload();*/
                }
                
                loaded_scene = scenes[id];
                
                if(OnSceneLoad != null)
                    OnSceneLoad(loaded_scene);
                
                loaded_scene.Awake();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to load the scene! {e}");
            }
        }

        public static void AddEntityToScene(Entity entity)
        {
            if (EntityAdded != null)
            {
                EntityAdded(entity);
            }
            
            loaded_scene?.AddEntity(entity);
        }
        
        public static Entity GetEntityFromScene(string name)
        {
            return loaded_scene?.GetEntity(name);
        }
        
        public static Entity GetEntityFromScene(Entity entity)
        {
            return loaded_scene?.GetEntity(entity);
        }

        public static List<Entity> GetSceneEntities()
        {
            return loaded_scene?.SceneEntities;
        }
        
        public static void RemoveEntityFromScene(Component component)
        {
            Entity remove = loaded_scene?.RemoveEntity(component.Parent);
            
            if (EntityRemoved != null)
            {
                EntityRemoved(remove);
            }
        }
        public static void RemoveEntityFromScene(Entity entity)
        {
            Entity remove = loaded_scene?.RemoveEntity(entity);
            
            if (EntityRemoved != null)
            {
                EntityRemoved(remove);
            }
        }

        public static bool hasLoadedScene => loaded_scene != null;
        
        public static void CloseScene()
        {
            loaded_scene?.CloseScene();
        }
    }   
}