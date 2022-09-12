using System.Reflection;
using ImGuiNET;

namespace NekinuSoft.Editor;

public class PropertiesPanel : IEditorPanel
{
    //The id of selected component
    private int componentSelection;
    //the names of all components
    private List<string> string_comp;
    //Gets the type of all components
    private List<Type> comp;
    //a list of all components
    private List<TreeNodeComponent> components;
    private bool addingComponent = false;

    //The selected entity
    private Entity selectedEntity;
    
    public override void Init()
    {
        components = new List<TreeNodeComponent>();
        HierarchyPanel.ItemSelected += HierarchyPanelOnItemSelected;

        string_comp = new List<string>();
        comp = new List<Type>();
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type component in assembly.GetTypes().Where(my => my.IsClass && !my.IsAbstract && my.IsSubclassOf(typeof(Component))))
            {
                string[] lines = component.ToString().Split(".");
                string type_name = lines[lines.Length - 1];
                string_comp.Add(type_name);
                comp.Add(component);
            }
        }

        string_comp = new List<string>(string_comp.OrderBy(x => x));
        comp = new List<Type>(comp.OrderBy(t => t.Name));
    }

    private void HierarchyPanelOnItemSelected(Entity entity)
    {
        selectedEntity = entity;
        components = ListComponents(entity);
    }

    private List<TreeNodeComponent> ListComponents(Entity entity)
    {
        List<TreeNodeComponent> components = new List<TreeNodeComponent>();
        if (entity != null)
        {
            for (int i = 0; i < entity.Components.Count; i++)
            {
                components.Add(new TreeNodeComponent(entity.Components[i]));
            }
        }

        return components;
    }
    
    public override void Render()
    {
        ImGui.Begin("Properties");
        
        if (selectedEntity != null)
        {
            ImGui.Text("Active");
            ImGui.SameLine();
            if (ImGui.Button(selectedEntity.IsActive ? "X" : " "))
            {
                selectedEntity.IsActive = !selectedEntity.IsActive;
            }
            ImGui.SameLine();
            string selectedName = selectedEntity.EntityName;
            ImGui.InputText("", ref selectedName, 20);

            System.Numerics.Vector3 pos = new System.Numerics.Vector3(selectedEntity.Transform.position.x, selectedEntity.Transform.position.y, selectedEntity.Transform.position.z);
            ImGui.DragFloat3("Position", ref pos, 0.1f);
            selectedEntity.Transform.position = new Vector3(pos.X, pos.Y, pos.Z);
            
            DrawComponents();

            ImGui.Spacing();
            
            if (ImGui.Button("Add Component"))
            {
                addingComponent = true;
            }

            if (addingComponent)
            {
                ImGui.BeginChild("Components");

                ImGui.Combo("Component List", ref componentSelection, string_comp.ToArray(), string_comp.Count);
                if (ImGui.Button("Add"))
                {
                    selectedEntity.AddComponent((Component) Activator.CreateInstance(comp[componentSelection]));
                    addingComponent = false;
                    components = ListComponents(selectedEntity);
                }
                
                ImGui.EndChild();
            }
        }
        
        ImGui.End();
    }

    private void DrawComponents()
    {
        for (int i = 0; i < components.Count; i++)
        {
            components[i].Render();
        }
    }
}