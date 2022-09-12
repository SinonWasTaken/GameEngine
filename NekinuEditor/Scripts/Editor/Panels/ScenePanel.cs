using ImGuiNET;

namespace NekinuSoft.Editor
{
    public class ScenePanel : IEditorPanel
    {
        //Gets the selected entity
        private Entity selectedEntity;
        private ImGuiWindowFlags flags;

        public override void Init()
        {
            HierarchyPanel.ItemSelected += HierarchyPanelOnItemSelected;
            //should make this panel not interact with the scrollwheel on the mouse
            flags = ImGuiWindowFlags.NoScrollbar;
        }

        //Updates the selected entity from the hierarchy
        private void HierarchyPanelOnItemSelected(Entity entity)
        {
            selectedEntity = entity;
        }

        public override void Render()
        {
            ImGui.Begin("Scene", flags);
            
            //Renders the scene to an editor window
            ImGui.Image((IntPtr) Editor_Window.buffer.colorBuffer, ImGui.GetWindowSize(), System.Numerics.Vector2.One, System.Numerics.Vector2.Zero);
            
            //If the user clicks on the scene panel, then the selected entity is set to null
            if (ImGui.IsItemClicked())
            {
                Console.WriteLine("Remove");
                Debug.WriteLog("Remove");
                HierarchyPanel.SelectEntity(null);
            }
            
            ImGui.End();
        }
    }
}