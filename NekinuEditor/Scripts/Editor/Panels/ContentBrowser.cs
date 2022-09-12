using ImGuiNET;

namespace NekinuSoft.Editor
{
    //Will be used to display all assets from a new project, in the editor
    public class ContentBrowser : IEditorPanel
    {
        public override void Init()
        {
        }

        public override void Render()
        {
            ImGui.Begin("Content");
            
            ImGui.End();
        }
    }
}