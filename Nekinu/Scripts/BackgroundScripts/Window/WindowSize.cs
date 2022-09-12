namespace NekinuSoft;

//Contains the width, height and the aspect ratio of the window
public static class WindowSize
{
    private static int height;
    private static int width;

    private static float aspectRatio;
    
    public static void UpdateSize(int x, int y)
    {
        width = x;
        height = y;

        aspectRatio = width * 1f / height * 1f;
    }

    public static int Height => height;
    public static int Width => width;

    public static float AspectRatio => aspectRatio;
}