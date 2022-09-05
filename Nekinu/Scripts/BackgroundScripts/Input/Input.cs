using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace NekinuSoft
{
    public class Input
    {
        //The new position of the mouse, compared to the last
        private static int delta_mouse_x, delta_mouse_y;

        //The mouse position on the screen
        private static int mouse_x, mouse_y;

        //All keys down at the current moment
        private static List<int> keys_down;
        
        //All keys pressed in the last frame
        private static List<int> keys_pressed;

        //All mouse buttons down currently 
        private static List<int> mouse_button_down;
        
        //All mouse buttons pressed in the last frame
        private static List<int> mouse_button_pressed;
        
        //The scroll value of the mouse
        private static int scroll;

        //Default constructor
        public Input(GameWindow window)
        {
            keys_down = new List<int>();
            keys_pressed = new List<int>();
            
            mouse_button_down = new List<int>();
            mouse_button_pressed = new List<int>();
            
            //Subscribes the methods to an event. MouseUp, MouseDown
            window.MouseUp += WindowOnMouseUp;
            window.MouseDown += WindowOnMouseDown;
            
            window.MouseMove += WindowOnMouseMove;

            window.KeyDown += WindowOnKeyDown;
            window.KeyUp += WindowOnKeyUp;
        }

        //Checks if a mouse button has been pressed
        public static bool is_mouse_button_pressed(MouseButton buttons)
        {
            //Check if the mouse button has been pressed ()
            for (int j = 0; j < mouse_button_pressed.Count; j++)
            {
                if (mouse_button_pressed[j] == (int) buttons)
                {
                    return false;
                }
            }
            
            mouse_button_pressed.Add((int) buttons);
            return true;
        }

        //Checks if the mouse button is being held down
        public static bool is_mouse_button_down(MouseButton buttons)
        {
            for (int i = 0; i < mouse_button_down.Count; i++)
            {
                if (mouse_button_down[i] == (int) buttons)
                {
                    return true;
                }
            }

            return false;
        }
        
        public static bool is_key_pressed(Keys keys)
        {
            //Checks if a key has already been pressed
            for (int j = 0; j < keys_pressed.Count; j++)
            {
                if (keys_pressed[j] == (int) keys)
                {
                    return false;
                }
            }
            
            //Add the key to the list if it hasnt been pressed
            keys_pressed.Add((int) keys);
            return true;
        }

        //Checks if the key is being held down
        public static bool is_key_down(Keys keys)
        {
            for (int i = 0; i < keys_down.Count; i++)
            {
                if (keys_down[i] == (int) keys)
                {
                    return true;
                }
            }

            return false;
        }

        //Adds a mouse button to the list, when it is down
        private void WindowOnMouseDown(MouseButtonEventArgs obj)
        {
            mouse_button_down.Add((int) obj.Button);
        }

        //Removes a mouse button to the list, when it is no longer pressed
        private void WindowOnMouseUp(MouseButtonEventArgs obj)
        {
            mouse_button_down.Remove((int) obj.Button);
            mouse_button_pressed.Remove((int) obj.Button);
        }
        
        //Called when a key is no longer pressed
        private void WindowOnKeyUp(KeyboardKeyEventArgs obj)
        {
            keys_down.Remove((int) obj.Key);
            keys_pressed.Remove((int) obj.Key);
        }

        //Called when a key is held down
        private void WindowOnKeyDown(KeyboardKeyEventArgs obj)
        {
            if (!is_key_already_pressed((int) obj.Key))
            {
                keys_down.Add((int) obj.Key);
            }
        }

        //Checks if a key is being pressed
        private bool is_key_already_pressed(int key)
        {
            for (int i = 0; i < keys_down.Count; i++)
            {
                if (keys_down[i] == key)
                {
                    return true;
                }
            }

            return false;
        }

        //Called when the mouse moves
        private void WindowOnMouseMove(MouseMoveEventArgs obj)
        {
            int n_delta_mouse_x = (int) obj.DeltaX;
            int n_delta_mouse_y = (int) obj.DeltaY;

            delta_mouse_x = n_delta_mouse_x;
            delta_mouse_y = n_delta_mouse_y;

            mouse_x = (int) obj.X;
            mouse_y = (int) obj.Y;
        }

        public static int Get_Mouse_X => mouse_x;
        public static int Get_Mouse_Y => mouse_y;

        public static float Get_Mouse_Delta_X
        {
            get
            {
                int v = delta_mouse_x;
                delta_mouse_x = 0;
                return (float) (v * Time.deltaTime);
            }
        }

        public static float Get_Mouse_Delta_Y
        {
            get
            {
                int v = delta_mouse_y;
                delta_mouse_y = 0;
                return (float) (v * Time.deltaTime);
            }
        }
    }
}