using ImGuiNET;

namespace NekinuSoft.Editor
{
    //A editor window that displays debug information
    public class DebugPanel : IEditorPanel
    {
        private float height;

        public override void Init()
        {
            height = 0;

        }

        public override void Render()
        {
            //Sets the window padding to zero
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new System.Numerics.Vector2(0, 0));
            //Creates the editor window
            if (ImGui.Begin("Debug"))
            {
                //Changes the color of a child
                ImGui.PushStyleColor(ImGuiCol.ChildBg, new System.Numerics.Vector4(0.3234375f, 0.3703125f, 0.52890625f, 1));
                
                //Creates a button that will clear all debug information 
                if (ImGui.BeginChild("ClearRect", new System.Numerics.Vector2(ImGui.GetWindowWidth(), height), true))
                {
                    //Changes the buttons color
                    ImGui.PushStyleColor(ImGuiCol.Button, new System.Numerics.Vector4(0.09f, 0.09f, 0.09f, 1));
                    if (ImGui.Button("Clear", new System.Numerics.Vector2(80, height)))
                    {
                        //Clears debug information
                        Debug.Clear();
                    }
                    
                    ImGui.EndChild();
                    //Removes the button color
                    ImGui.PopStyleColor();
                }

                //Removes the child color
                ImGui.PopStyleColor();
                
                //If there are debug information
                if (Debug.Lines != null)
                {
                    //Gets the editor window height
                    float thisHeight = ImGui.GetWindowHeight() * 0.08f;

                    //If the height has changed
                    if (thisHeight != height)
                    {
                        //update the old height
                        height = thisHeight;
                    }
                    
                    //Indents children
                    ImGui.Indent(5);
                    //for each line in the debug class
                    for (int i = 0; i < Debug.Lines.Count; i++)
                    {
                        //Changes the color of the line, depending on the type of debug. Normal is white and error is red
                        ImGui.PushStyleColor(ImGuiCol.Text, Debug.Lines[i].Color);
                        ImGui.Text(Debug.Lines[i].Line);
                        ImGui.PopStyleColor();
                    }

                    //Removes the indent
                    ImGui.Unindent();
                    ImGui.End();
                }
            }

            ImGui.PopStyleVar();
        }
    }
}