using NekinuSoft;

public class Debug
{
    //A list of all debug logs
    private static List<DebugLines> lines;

    //Starts the debug log
    public static void InitDebugLogging()
    {
        lines = new List<DebugLines>();
    }
    
    //Writes a normal log to the list
    public static void WriteLog(object value)
    {
        lines.Add(new DebugLines(value.ToString(), DebugLines.DebugType.Normal));
    }
    
    //Used to write an error to the log list. Will appear red in the debug panel
    public static void WriteErrorLog(object value)
    {
        lines.Add(new DebugLines(value.ToString(), new System.Numerics.Vector4(1, 0, 0, 1), DebugLines.DebugType.Error));
    }
    
    public static List<DebugLines> Lines => lines;

    //Clears the log of all written logs
    public static void Clear()
    {
        lines.Clear();
    }
}

//A class that determines the type of debug log
public class DebugLines
{
    //Is the log a normal log, or an error
    public enum DebugType
    {
        Normal,
        Error
    }
    
    //The written log
    private string line;
    //The color of the log
    private System.Numerics.Vector4 color;
    //The type of log. Error or normal
    private DebugType type;

    //Constructors
    public DebugLines(string line, DebugType type)
    {
        this.line = line;
        color = new System.Numerics.Vector4(1, 1, 1, 1);
        this.type = type;
    }
    
    public DebugLines(string line, System.Numerics.Vector4 color, DebugType type)
    {
        this.line = line;
        this.color = color;
        this.type = type;
    }

    public string Line => line;

    public System.Numerics.Vector4 Color => color;

    public DebugType Type => type;
}