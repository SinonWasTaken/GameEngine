using ImGuiNET;

namespace NekinuSoft.Editor
{
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