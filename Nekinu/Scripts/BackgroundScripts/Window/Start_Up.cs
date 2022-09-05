namespace NekinuSoft
{
    public class Start_Up
    {
        //A public method that allows outside projects to start the engine
        public static void Main(string[] args)
        {
            Window w = new Window("Title", 800, 600, false);
            
            w.Start();
        }

        public static Window Start(string windowTitle, int windowWidth, int windowHeight, bool isFullScreen)
        {
            return new Window(windowTitle, windowWidth, windowHeight, isFullScreen);
        }
    }
}