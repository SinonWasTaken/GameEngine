using System.Reflection;
using ImGuiNET;
using Nekinu;

namespace NekinuSoft.Editor;

//A class that renders a component on the selected entity
public class TreeNodeComponent
{
    //The component
    private Component component;

    //only renders the information if this is true
    private bool isOpen;
    
    public TreeNodeComponent(Component component)
    {
        this.component = component;
        isOpen = false;
    }

    //Default render method
    public void Render()
    {
        RenderComponentDetails();
    }

    //Renders the component detail
    private void RenderComponentDetails()
    {
        //Pushes the id of the component in the entity. If two of the same components exists, both will be rendered separately
        ImGui.PushID(findComponentIndex());
        
        //Enables or disables a component
        if (ImGui.Button(component.IsActive ? "X" : " "))
        {
            component.IsActive = !component.IsActive;
        }
        
        //Removes the id from the stack
        ImGui.PopID();
        
        //Makes the following code render on the same line as the previous
        ImGui.SameLine();
        
        //Gets the name of the component
        string[] lines = component.GetType().ToString().Split(".");
        //Renders a button with the name of the component
        if (ImGui.Button($"{lines[lines.Length - 1]}"))
        {
            isOpen = !isOpen;
        }
            
        //Render the information if the fold is open 
        if (isOpen)
        {
            Type type = component.GetType();
            
            drawFields(component, type);
        }
    }

    //Gets the index of a component
    private int findComponentIndex()
    {
        for (int i = 0; i < HierarchyPanel.selectedEntity.Components.Count; i++)
        {
            if (component == HierarchyPanel.selectedEntity.Components[i])
            {
                return i;
            }
        }

        return -1;
    }

    //Draws all fields in a class that meet a certain requirement
    private void drawFields(Component component, Type type)
    {
        FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            
        Array.Sort(fields, (x, y) => String.Compare(x.Name, y.Name));
            
        for (int i = 0; i < fields.Length; i++)
        {
            FieldInfo info = fields[i];

            if (Attribute.IsDefined(info, typeof(SerializedPropertyAttribute)) || info.IsPublic)
            {
                checkFieldType(component, info, fields);
            }
        }

        type = component.GetType().BaseType;
        while (type != typeof(Component))
        {
            fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo info = fields[i];

                if (Attribute.IsDefined(info, typeof(SerializedPropertyAttribute)))
                {
                    checkFieldType(component, info, fields);
                }
            }
                
            type = type.BaseType;
        }
    }

    //Checks every field in a class, and determine if it can be show in the editor
    private void checkFieldType(Component c, FieldInfo info, FieldInfo[] infos)
    {
        try
        {
            if (info.FieldType == typeof(int))
            {
                int v = (int) infos.Single(pi => pi.Name == info.Name).GetValue(c);
                ImGui.DragInt($"{info.Name}", ref v);

                infos.Single(pi => pi.Name == info.Name).SetValue(c, v);
            }
            else if (info.FieldType == typeof(string))
            {
                string v = (string) infos.Single(pi => pi.Name == info.Name).GetValue(c);
                //ImGui.InputText("Text", ref v, (uint) v.Length);

                ImGui.Text(v);
                
                infos.Single(pi => pi.Name == info.Name).SetValue(c, v);
            }
            else if (info.FieldType == typeof(float))
            {
                float v = (float) infos.Single(pi => pi.Name == info.Name).GetValue(c);
                ImGui.DragFloat($"{info.Name}", ref v, 0.001f, float.MinValue, float.MaxValue);

                infos.Single(pi => pi.Name == info.Name).SetValue(c, v);
            }
            else if (info.FieldType == typeof(Vector2))
            {
                Vector2 ve = (Vector2) infos.Single(pi => pi.Name == info.Name).GetValue(c);
                System.Numerics.Vector2 v = new System.Numerics.Vector2(ve.x, ve.y);
                ImGui.DragFloat2($"{info.Name}", ref v, 0.001f, float.MinValue, float.MaxValue);

                ve.ConvertSystemVector(v);
                infos.Single(pi => pi.Name == info.Name).SetValue(c, ve);
            }
            else if (info.FieldType == typeof(Vector3))
            {
                Vector3 ve = (Vector3) infos.Single(pi => pi.Name == info.Name).GetValue(c);
                System.Numerics.Vector3 v = new System.Numerics.Vector3(ve.x, ve.y, ve.z);
                ImGui.DragFloat3($"{info.Name}", ref v, 0.001f, float.MinValue, float.MaxValue);

                ve = new Vector3(v.X, v.Y, v.Z);
                infos.Single(pi => pi.Name == info.Name).SetValue(c, ve);
            }
            else if (info.FieldType == typeof(Vector4))
            {
                Vector4 ve = (Vector4) infos.Single(pi => pi.Name == info.Name).GetValue(c);
                System.Numerics.Vector4 v = new System.Numerics.Vector4(ve.x, ve.y, ve.z, ve.w);
                ImGui.DragFloat4($"{info.Name}", ref v, 0.001f, float.MaxValue, float.MaxValue);

                ve = new Vector4(v.X, v.Y, v.Z, v.W);

                infos.Single(pi => pi.Name == info.Name).SetValue(c, ve);
            }
            else if (info.FieldType == typeof(bool))
            {
                bool value = (bool) infos.Single(pi => pi.Name == info.Name).GetValue(c);

                ImGui.Checkbox($"{info.Name}", ref value);
                infos.Single(pi => pi.Name == info.Name).SetValue(c, value);
            }
        }
        catch (Exception e)
        {
            Debug.WriteErrorLog($"Error drawing editor field: {e}");
        }
    }

    //Same as the method above, but only for properties in a class. Not being used
    private void drawProperties(Component c, Type t)
    {
        for (int prop = 0; prop < t.GetProperties().Length; prop++)
        {
            PropertyInfo info = t.GetProperties()[prop];

            PropertyInfo[] infos = t.GetProperties();

            if (info.PropertyType == typeof(int))
            {
                if (infos.Single(pi => pi.Name == info.Name).GetSetMethod() != null)
                {
                    int v = (int) infos.Single(pi => pi.Name == info.Name).GetValue(c, null);
                    ImGui.DragInt($"{info.Name}", ref v);

                    infos.Single(pi => pi.Name == info.Name).SetValue(c, v);
                }
            }
            else if (info.PropertyType == typeof(string))
            {
                if (infos.Single(pi => pi.Name == info.Name).GetSetMethod() != null)
                {
                    string v = (string) infos.Single(pi => pi.Name == info.Name).GetValue(c, null);
                    ImGui.LabelText($"{info.Name}", v);

                    infos.Single(pi => pi.Name == info.Name).SetValue(c, v);
                }
            }
            else if (info.PropertyType == typeof(float))
            {
                if (infos.Single(pi => pi.Name == info.Name).GetSetMethod() != null)
                {
                    float v = (float) infos.Single(pi => pi.Name == info.Name).GetValue(c, null);
                    ImGui.DragFloat($"{info.Name}", ref v, 0.1f);

                    infos.Single(pi => pi.Name == info.Name).SetValue(c, v);
                }
            }
            else if (info.PropertyType == typeof(Vector2))
            {
                if (infos.Single(pi => pi.Name == info.Name).GetSetMethod() != null)
                {
                    Vector2 ve = (Vector2) infos.Single(pi => pi.Name == info.Name).GetValue(c, null);
                    System.Numerics.Vector2 v = new System.Numerics.Vector2(ve.x, ve.y);
                    ImGui.DragFloat2($"{info.Name}", ref v, 0.1f);

                    ve.ConvertSystemVector(v);
                    infos.Single(pi => pi.Name == info.Name).SetValue(c, ve);
                }
            }
            else if (info.PropertyType == typeof(Vector3))
            {
                if (infos.Single(pi => pi.Name == info.Name).GetSetMethod() != null)
                {
                    Vector3 ve = (Vector3) infos.Single(pi => pi.Name == info.Name).GetValue(c, null);
                    System.Numerics.Vector3 v = new System.Numerics.Vector3(ve.x, ve.y, ve.z);

                    ImGui.DragFloat3($"{info.Name}", ref v, 0.1f);

                    ve = new Vector3(v.X, v.Y, v.Z);

                    infos.Single(pi => pi.Name == info.Name).SetValue(c, ve);
                }
            }
            else if (info.PropertyType == typeof(Vector4))
            {
                if (infos.Single(pi => pi.Name == info.Name).GetSetMethod() != null)
                {
                    Vector4 ve = (Vector4) infos.Single(pi => pi.Name == info.Name).GetValue(c, null);

                    System.Numerics.Vector4 v = new System.Numerics.Vector4(ve.x, ve.y, ve.z, ve.w);
                    ImGui.DragFloat4($"{info.Name}", ref v, 0.1f);

                    ve = new Vector4(v.X, v.Y, v.Z, v.W);

                    infos.Single(pi => pi.Name == info.Name).SetValue(c, ve);
                }
            }
            else if (info.PropertyType == typeof(bool))
            {
                if (infos.Single(pi => pi.Name == info.Name).GetSetMethod() != null)
                {
                    bool value = (bool) infos.Single(pi => pi.Name == info.Name).GetValue(c);

                    ImGui.Checkbox($"{info.Name}", ref value);

                    infos.Single(pi => pi.Name == info.Name).SetValue(c, value);
                }
            }
        }
    }
}