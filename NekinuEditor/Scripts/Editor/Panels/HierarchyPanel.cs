using ImGuiNET;
using NekinuSoft.Scene_Manager;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace NekinuSoft.Editor
{
    public class HierarchyPanel : IEditorPanel
    {
        public delegate void itemSelected(Entity entity);
        public static event itemSelected ItemSelected;
        
        private ImGuiTreeNodeFlags flags;

        private Scene loadedScene;
        
        public static Entity selectedEntity;

        private List<TreeNodeEntity> sceneEntitiesTree;
        
        public override void Init()
        {
            sceneEntitiesTree = new List<TreeNodeEntity>();
            
            SceneManager.OnSceneLoad += SceneManagerOnSceneLoad;
            SceneManager.EntityAdded += SceneManagerOnEntityAdded;
            SceneManager.EntityRemoved += SceneManagerOnEntityRemoved;
            flags = ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.OpenOnDoubleClick;
        }

        public static void SelectEntity(Entity entity)
        {
            if(ItemSelected != null)
                ItemSelected(entity);
            
            selectedEntity = entity;
        }
        
        private void SceneManagerOnEntityRemoved(Entity entity)
        {
            for (int i = 0; i < sceneEntitiesTree.Count; i++)
            {
                if (sceneEntitiesTree[i].Entity == entity)
                {
                    sceneEntitiesTree.Remove(sceneEntitiesTree[i]);
                }
            }
        }

        private void SceneManagerOnEntityAdded(Entity entity)
        {
            if (entity.Parent != null)
            {
                TreeNodeEntity parent = findParent(sceneEntitiesTree, entity);

                if (parent != null)
                {
                    parent.AddChild(new TreeNodeEntity(entity));
                }
                else
                {
                    Debug.WriteErrorLog(
                        $"Child {entity.EntityName} has a parent: {parent.Entity.EntityName}, but it doesn't exist in the list!");
                }
            }
            else
            {
                sceneEntitiesTree.Add(new TreeNodeEntity(entity));
            }
        }

        private void SceneManagerOnSceneLoad(Scene scene)
        {
            loadedScene = scene;

            selectedEntity = null;
            
            sceneEntitiesTree = new List<TreeNodeEntity>();

            sceneEntitiesTree = SortSceneEntities(loadedScene.SceneEntities);
        }

        private List<TreeNodeEntity> SortSceneEntities(List<Entity> sceneEntities)
        {
            List<TreeNodeEntity> parents = new List<TreeNodeEntity>();

            for (int i = 0; i < sceneEntities.Count; i++)
            {
                if (sceneEntities[i].Parent == null)
                {
                    parents.Add(new TreeNodeEntity(sceneEntities[i]));
                }
            }

            for (int i = 0; i < sceneEntities.Count; i++)
            {
                if (sceneEntities[i].Parent != null)
                {
                    TreeNodeEntity parent = findParent(parents, sceneEntities[i]);
                    if (parent != null)
                    {
                        parent.AddChild(new TreeNodeEntity(sceneEntities[i]));
                    }
                    else
                    {
                        Debug.WriteErrorLog($"Child {sceneEntities[i].EntityName} has a parent: {sceneEntities[i].Parent.EntityName}, but it doesn't exist in the list!");
                    }
                }
            }

            return parents;
        }

        private TreeNodeEntity findParent(List<TreeNodeEntity> parent, Entity child)
        {
            for (int i = 0; i < parent.Count; i++)
            {
                if(parent[i].Entity == child.Parent)
                {
                    return parent[i];
                }
            }

            for (int i = 0; i < parent.Count; i++)
            {
                for (int j = 0; j < parent[i].Children.Count; j++)
                {
                    TreeNodeEntity childParent = findParent(parent[i].Children, child);
                    if (childParent != null)
                    {
                        return childParent;
                    }
                }
            }

            return null;
        }

        public override void Render()
        {
            ImGui.Begin("Hierarchy");

            if (loadedScene != null)
            {
                for (int i = 0; i < sceneEntitiesTree.Count; i++)
                {
                    sceneEntitiesTree[i].Render();
                }
            }

            if (selectedEntity != null)
            {
                if (Input.is_key_down(Keys.Delete))
                {
                    SceneManager.RemoveEntityFromScene(selectedEntity);
                    selectedEntity = null;
                }
            }
            
            ImGui.End();
        }
    }
}