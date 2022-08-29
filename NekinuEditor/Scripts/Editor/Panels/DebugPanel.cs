using ImGuiNET;

namespace NekinuSoft.Editor
{
    public class DebugPanel : IEditorPanel
    {
        private float height;

        public override void Init()
        {
            height = 0;

        }

        public override void Render()
        {
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new System.Numerics.Vector2(0, 0));
            if (ImGui.Begin("Debug"))
            {
                ImGui.PushStyleColor(ImGuiCol.ChildBg, new System.Numerics.Vector4(0.3234375f, 0.3703125f, 0.52890625f, 1));
                
                if (ImGui.BeginChild("ClearRect", new System.Numerics.Vector2(ImGui.GetWindowWidth(), height), true))
                {
                    ImGui.PushStyleColor(ImGuiCol.Button, new System.Numerics.Vector4(0.09f, 0.09f, 0.09f, 1));
                    if (ImGui.Button("Clear", new System.Numerics.Vector2(80, height)))
                    {
                        Debug.Clear();
                    }

                    ImGui.EndChild();
                    ImGui.PopStyleColor();
                }

                ImGui.PopStyleColor();
                
                if (Debug.Lines != null)
                {
                    float thisHeight = ImGui.GetWindowHeight() * 0.08f;

                    if (thisHeight != height)
                    {
                        height = thisHeight;
                    }

                    ImGui.Indent(5);
                    for (int i = 0; i < Debug.Lines.Count; i++)
                    {
                        ImGui.PushStyleColor(ImGuiCol.Text, Debug.Lines[i].Color);
                        ImGui.Text(Debug.Lines[i].Line);
                        ImGui.PopStyleColor();
                    }

                    ImGui.Unindent();
                    ImGui.End();
                }
            }

            ImGui.PopStyleVar();
        }
    }
}