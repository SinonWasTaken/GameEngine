using Nekinu;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace NekinuSoft.UI
{
    public class Button : UI_Class
    {
        //The normal color of the button
        [SerializedProperty] private Vector4 normal_color;
        //The color of the button when the mouse is over the button
        [SerializedProperty] private Vector4 highlighted_color;
        //The color of the button when pressing the button
        [SerializedProperty] private Vector4 interact_color;

        //The color being displayed
        private Vector4 out_color;

        //is the mouse inside the button
        private bool inside;
        //is the player pressing the button
        private bool interacting;

        //the type of events that the button can do. OnMouseDown, OnMouseUp, OnMouseEnter etc...
        private List<Button_Interaction_Events> events;

        
        public Button() : base(new Vector4(1, 1, 1, 1))
        {
            events = new List<Button_Interaction_Events>();
            events.Add(new Button_Interaction_Events(Button_Interaction_Events.InteractionType.OnButtonDown,
                () => Console.WriteLine("Doesn't do anything")));

            normal_color = new Vector4(1, 1, 1, 1);

            highlighted_color = new Vector4(1, 0, 0, 0.85f);
            interact_color = new Vector4(1, 0, 1, 0.85f);
        }

        public Button(string texture_name, string texture_extension, Vector4 color,
            params Button_Interaction_Events[] action) : base(texture_name, texture_extension, color)
        {
            events = new List<Button_Interaction_Events>();
            events.AddRange(action);

            normal_color = new Vector4(1, 1, 1, 1);

            highlighted_color = new Vector4(1, 0, 0, 0.85f);
            interact_color = new Vector4(1, 0, 1, 0.85f);
        }

        public override void Is_Mouse_Over(Camera camera)
        {
            //Gets the center of the screen
            int half_screen_width = (WindowSize.Width / 2);
            int half_screen_height = (WindowSize.Height / 2);

            //The size of the ui object
            int half_ui_width = (int) (WindowSize.Width * Parent.Transform.scale.x) / 2;
            int half_ui_height = (int) (WindowSize.Height * Parent.Transform.scale.y) / 2;

            //If the mouse is within the button
            if (Input.Get_Mouse_X >= half_screen_width + Parent.Transform.position.x - half_ui_width &&
                Input.Get_Mouse_X <= half_screen_width + Parent.Transform.position.x + half_ui_width)
            {
                if (Input.Get_Mouse_Y >= half_screen_height + Parent.Transform.position.y - half_ui_height &&
                    Input.Get_Mouse_Y <= half_screen_height + Parent.Transform.position.y + half_ui_height)
                {
                    //the mouse is inside
                    inside = true;
                    //the color is set to the highlighted color
                    out_color = highlighted_color;
                }
                else
                {
                    //the mouse is not inside
                    inside = false;
                    //the color is normal
                    out_color = normal_color;
                }
            }
            else
            {
                inside = false;
                out_color = normal_color;
            }

            if (inside)
            {
                //If the left mouse is pressed and the mouse is inside the button
                if (Input.is_mouse_button_pressed(MouseButton.Left))
                {
                    //the color is now the interactive color
                    out_color = interact_color;

                    //Checks each button event and actives them if the correct input is given 
                    foreach (Button_Interaction_Events interaction in events)
                    {
                        if (interaction.Type == Button_Interaction_Events.InteractionType.OnButtonDown)
                        {
                            interaction.InteractEvent.Invoke();
                        }
                    }
                }
            }
        }

        public override Vector4 Color
        {
            get { return out_color; }
        }

    }

    public class Button_Interaction_Events
    {
        public enum InteractionType
        {
            OnButtonDown,
            OnButtonUp,
            OnMouseExit,
            OnMouseEnter
        }

        private InteractionType type;
        //Code to be executed
        private Action interact_event;

        public Button_Interaction_Events(InteractionType type, Action interactEvent)
        {
            this.type = type;
            interact_event = interactEvent;
        }

        public InteractionType Type => type;
        public Action InteractEvent => interact_event;
    }
}