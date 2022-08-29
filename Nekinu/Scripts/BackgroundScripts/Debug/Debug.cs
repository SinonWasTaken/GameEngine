using NekinuSoft;

public class Debug
{
    private static List<DebugLines> lines;

    public static void InitDebugLogging()
    {
        lines = new List<DebugLines>();
    }
    
    public static void WriteLog(string value)
    {
        lines.Add(new DebugLines(value, DebugLines.DebugType.Normal));
    }
    public static void WriteLog(object value)
    {
        lines.Add(new DebugLines(value.ToString(), DebugLines.DebugType.Normal));
    }
    public static void WriteLog(float value)
    {
        lines.Add(new DebugLines(value.ToString(), DebugLines.DebugType.Normal));
    }
    public static void WriteLog(int value)
    {
        lines.Add(new DebugLines(value.ToString(), DebugLines.DebugType.Normal));
    }
    public static void WriteLog(Vector2 value)
    {
        lines.Add(new DebugLines(value.ToString(), DebugLines.DebugType.Normal));
    }
    public static void WriteLog(Vector3 value)
    {
        lines.Add(new DebugLines(value.ToString(), DebugLines.DebugType.Normal));
    }
    public static void WriteLog(Vector4 value)
    {
        lines.Add(new DebugLines(value.ToString(), DebugLines.DebugType.Normal));
    }
    
    public static void WriteErrorLog(string value)
    {
        lines.Add(new DebugLines(value, new System.Numerics.Vector4(1, 0, 0, 1), DebugLines.DebugType.Error));
    }
    public static void WriteErrorLog(object value)
    {
        lines.Add(new DebugLines(value.ToString(), new System.Numerics.Vector4(1, 0, 0, 1), DebugLines.DebugType.Error));
    }
    public static void WriteErrorLog(float value)
    {
        lines.Add(new DebugLines(value.ToString(), new System.Numerics.Vector4(1, 0, 0, 1), DebugLines.DebugType.Error));
    }
    public static void WriteErrorLog(int value)
    {
        lines.Add(new DebugLines(value.ToString(), new System.Numerics.Vector4(1, 0, 0, 1), DebugLines.DebugType.Error));
    }
    public static void WriteErrorLog(Vector2 value)
    {
        lines.Add(new DebugLines(value.ToString(), new System.Numerics.Vector4(1, 0, 0, 1), DebugLines.DebugType.Error));
    }
    public static void WriteErrorLog(Vector3 value)
    {
        lines.Add(new DebugLines(value.ToString(), new System.Numerics.Vector4(1, 0, 0, 1), DebugLines.DebugType.Error));
    }
    public static void WriteErrorLog(Vector4 value)
    {
        lines.Add(new DebugLines(value.ToString(), new System.Numerics.Vector4(1, 0, 0, 1), DebugLines.DebugType.Error));
    }

    public static List<DebugLines> Lines => lines;

    public static void Clear()
    {
        lines.Clear();
    }
}

public class DebugLines
{
    public enum DebugType
    {
        Normal,
        Error
    }
    
    private string line;
    private System.Numerics.Vector4 color;
    private DebugType type;

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