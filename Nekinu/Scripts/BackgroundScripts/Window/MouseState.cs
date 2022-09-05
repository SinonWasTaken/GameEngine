namespace NekinuSoft
{
    //Doesn't work properly. Need to fix
    public static class MouseState
    {
        private static bool is_visible = false;

        public static void Init()
        {
            Window.window.CursorVisible = false;
            Window.window.Grab_Cursor();
        }

        public static void Set_Mouse_Visible(bool value)
        {
            is_visible = value;

            if (is_visible)
            {
                Window.window.CursorVisible = true;
                Window.window.Release_Cursor();
            }
            else
            {
                Window.window.CursorVisible = false;
                Window.window.Grab_Cursor();
            }
        }

        public static bool IsVisible => is_visible;
    }
}