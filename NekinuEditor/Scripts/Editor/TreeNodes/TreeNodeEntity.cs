using ImGuiNET;

namespace NekinuSoft.Editor;

public class TreeNodeEntity
{
    private Entity entity;
    private List<TreeNodeEntity> children; 
    private bool isOpen;

    public TreeNodeEntity(Entity entity)
    {
        this.entity = entity;
        isOpen = false;
        children = new List<TreeNodeEntity>();
    }

    public void AddChild(TreeNodeEntity child)
    {
        children.Add(child);
    }
    public void RemoveChild(TreeNodeEntity child)
    {
        children.Remove(child);
    }

    public void Render()
    {
        if (HierarchyPanel.selectedEntity != entity)
        {
            ImGui.PushStyleColor(ImGuiCol.Button, new System.Numerics.Vector4(0f, 0f, 0f, 0f));
        }
        else
        {
            ImGui.PushStyleColor(ImGuiCol.Button, new System.Numerics.Vector4(0.4f, 0.4f, 0.9f, 1f));
        }

        string buttonText = !isOpen ? "->" : "-V";

        if (ImGui.Button($"{buttonText} {entity.EntityName}"))
        {
            isOpen = !isOpen;
        }
            
        ImGui.PopStyleColor();

        if (ImGui.IsItemClicked())
        {
            HierarchyPanel.SelectEntity(entity);
        }

        if (isOpen)
        {
            ImGui.Indent(10);
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Render();
            }
            ImGui.Indent(-10);
        }
    }

    public Entity Entity => entity;

    public List<TreeNodeEntity> Children => children;
}