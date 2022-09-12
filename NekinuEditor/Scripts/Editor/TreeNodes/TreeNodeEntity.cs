using ImGuiNET;

namespace NekinuSoft.Editor;

//Class that renders the children attached to a parent entity
public class TreeNodeEntity
{
    //The parent entity
    private Entity entity;
    //All children of the entity
    private List<TreeNodeEntity> children;
    //if the entity tab open
    private bool isOpen;

    public TreeNodeEntity(Entity entity)
    {
        this.entity = entity;
        isOpen = false;
        children = new List<TreeNodeEntity>();
    }

    //Adds a child to the entity
    public void AddChild(TreeNodeEntity child)
    {
        children.Add(child);
    }
    //Removes a child from the entity
    public void RemoveChild(TreeNodeEntity child)
    {
        children.Remove(child);
    }

    //Renders the entity and its children
    public void Render()
    {
        //Changes the color of the entity editor tab if its selected
        if (HierarchyPanel.selectedEntity != entity)
        {
            ImGui.PushStyleColor(ImGuiCol.Button, new System.Numerics.Vector4(0f, 0f, 0f, 0f));
        }
        else
        {
            ImGui.PushStyleColor(ImGuiCol.Button, new System.Numerics.Vector4(0.4f, 0.4f, 0.9f, 1f));
        }

        //Changes text depending on the entity's open state
        string buttonText = !isOpen ? "->" : "-V";

        //Creates an editor button
        if (ImGui.Button($"{buttonText} {entity.EntityName}"))
        {
            isOpen = !isOpen;
        }
            
        //Removes the button color
        ImGui.PopStyleColor();

        //If the current entity is clicked
        if (ImGui.IsItemClicked())
        {
            //the select the entity and display its information
            HierarchyPanel.SelectEntity(entity);
        }

        //If this entity tab is open
        if (isOpen)
        {
            ImGui.Indent(10);
            //render all children
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