namespace NekinuSoft.Math
{
    public class Math
    {
        public const float PI = 3.14159265359f;

        public const float ToRadians = 0.01745329251f;
        public const float ToDegrees = 57.2957795131f;

        public static float Abs(float value)
        {
            if (value < 0)
                return value * -1f;

            return value;
        }

        public static int Abs(int value)
        {
            if (value < 0)
                return value * -1;

            return value;
        }

        public static int Max(int a, int b)
        {
            return a > b ? a : b;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value > max)
            {
                return max;
            }
            else if (value < min)
            {
                return min;
            }
            else
            {
                return value;
            }
        }
        
        public static float Clamp(float value, float min, float max)
        {
            if (value > max)
            {
                return max;
            }
            else if (value < min)
            {
                return min;
            }
            else
            {
                return value;
            }
        }

        public static int Round(float value)
        {
            int clamped_value = (int) value;

            float v = value - clamped_value;

            if (v > 0.5f)
            {
                return clamped_value + 1;
            }
            else
            {
                return clamped_value;
            }
        }
    }
}