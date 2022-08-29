using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace NekinuSoft
{
    public class Input
    {
        private static int delta_mouse_x, delta_mouse_y;

        private static int mouse_x, mouse_y;

        private static List<int> keys_down;
        
        private static List<int> keys_pressed;

        private static List<int> mouse_button_down;
        
        private static List<int> mouse_button_pressed;
        
        private static int scroll;

        public Input(GameWindow window)
        {
            keys_down = new List<int>();
            keys_pressed = new List<int>();
            
            mouse_button_down = new List<int>();
            mouse_button_pressed = new List<int>();
            
            window.MouseUp += WindowOnMouseUp;
            window.MouseDown += WindowOnMouseDown;
            
            window.MouseMove += WindowOnMouseMove;

            window.KeyDown += WindowOnKeyDown;
            window.KeyUp += WindowOnKeyUp;
        }

        private void WindowOnMouseDown(MouseButtonEventArgs obj)
        {
            mouse_button_down.Add((int) obj.Button);
        }

        private void WindowOnMouseUp(MouseButtonEventArgs obj)
        {
            mouse_button_down.Remove((int) obj.Button);
            mouse_button_pressed.Remove((int) obj.Button);
        }

        public static bool is_mouse_button_pressed(MouseButton buttons)
        {
            for (int i = 0; i < mouse_button_down.Count; i++)
            {
                if (mouse_button_down[i] == (int) buttons)
                {
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
            }

            return false;
        }

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
            for (int i = 0; i < keys_down.Count; i++)
            {
                if (keys_down[i] == (int) keys)
                {
                    for (int j = 0; j < keys_pressed.Count; j++)
                    {
                        if (keys_pressed[j] == (int) keys)
                        {
                            return false;
                        }
                    }
                    keys_pressed.Add((int) keys);
                    return true;
                }
            }

            return false;
        }

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

        private void WindowOnKeyUp(KeyboardKeyEventArgs obj)
        {
            keys_down.Remove((int) obj.Key);
            keys_pressed.Remove((int) obj.Key);
        }

        private void WindowOnKeyDown(KeyboardKeyEventArgs obj)
        {
            if (!is_key_already_pressed((int) obj.Key))
            {
                keys_down.Add((int) obj.Key);
            }
        }

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