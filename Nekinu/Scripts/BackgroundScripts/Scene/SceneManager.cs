namespace NekinuSoft.Scene_Manager
{
    //Manages all scenes
    public static class SceneManager
    {
        //Event called when a scene is loaded
        public delegate void SceneLoad(Scene scene);
        public static event SceneLoad OnSceneLoad;

        //Event called when an entity is added or removed
        public delegate void EntityAdd(Entity entity);
        public static event EntityAdd EntityAdded;
        public static event EntityAdd EntityRemoved;
        
        //List of all active scenes
        private static List<Scene> scenes;
        
        //The current loaded scene
        private static Scene loaded_scene;
        
        //Initializes the scene list
        public static void InitSceneManager()
        {
            scenes = new List<Scene>();
        }

        //Updates the current scene, if the current scene isn't null
        public static void UpdateScene()
        {
            loaded_scene?.Update();
        }

        //Adds an array of scenes to the list
        public static void AddScene(params Scene[] scene_loading)
        {
            scenes.AddRange(scene_loading);
        }
        //Removes a scene from the list
        public static void RemoveScene(Scene scene)
        {
            scenes.Remove(scene);
        }
        
        //Loads a scene by its index in the scene list
        public static void LoadScene(int id)
        {
            try
            {
                if (loaded_scene != null)
                {
                    //Closes the old scene
                    loaded_scene.CloseScene();
                    
                    /*if(OnSceneUnload != null)
                        OnSceneUnload();*/
                }
                
                //Sets the loaded scene to the new scene
                loaded_scene = scenes[id];
                
                if(OnSceneLoad != null)
                    OnSceneLoad(loaded_scene);
                
                //Starts the scene
                loaded_scene.Awake();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to load the scene! {e}");
            }
        }

        //Adds an entity to the current scene
        public static void AddEntityToScene(Entity entity)
        {
            if (EntityAdded != null)
            {
                EntityAdded(entity);
            }
            
            loaded_scene?.AddEntity(entity);
        }
        
        //Gets an entity from the current scene
        public static Entity GetEntityFromScene(string name)
        {
            return loaded_scene?.GetEntity(name);
        }
        
        //Gets an entity from the current scene
        public static Entity GetEntityFromScene(Entity entity)
        {
            return loaded_scene?.GetEntity(entity);
        }

        //Gets the list of all entities in a scene
        public static List<Entity> GetSceneEntities()
        {
            return loaded_scene?.SceneEntities;
        }
        
        //Removes an entity from the current scene
        public static void RemoveEntityFromScene(Component component)
        {
            Entity remove = loaded_scene?.RemoveEntity(component.Parent);
            
            if (EntityRemoved != null)
            {
                EntityRemoved(remove);
            }
        }
        //Removes an entity from the current scene
        public static void RemoveEntityFromScene(Entity entity)
        {
            Entity remove = loaded_scene?.RemoveEntity(entity);
            
            if (EntityRemoved != null)
            {
                EntityRemoved(remove);
            }
        }

        public static bool hasLoadedScene => loaded_scene != null;
        
        //Closes the scene manager and the loaded scene
        public static void CloseScene()
        {
            loaded_scene?.CloseScene();
        }
    }   
}