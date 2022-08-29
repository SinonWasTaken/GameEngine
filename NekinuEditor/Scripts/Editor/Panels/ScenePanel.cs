using ImGuiNET;

namespace NekinuSoft.Editor
{
    public class ScenePanel : IEditorPanel
    {
        private Entity selectedEntity;
        private ImGuiWindowFlags flags;

        public override void Init()
        {
            HierarchyPanel.ItemSelected += HierarchyPanelOnItemSelected;
            flags = ImGuiWindowFlags.NoScrollbar;
        }

        private void HierarchyPanelOnItemSelected(Entity entity)
        {
            selectedEntity = entity;
        }

        public override void Render()
        {
            ImGui.Begin("Scene", flags);
            ImGui.Image((IntPtr) Editor_Window.buffer.colorBuffer, ImGui.GetWindowSize(), System.Numerics.Vector2.One, System.Numerics.Vector2.Zero);
            
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