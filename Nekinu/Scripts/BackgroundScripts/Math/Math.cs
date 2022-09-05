namespace NekinuSoft.Math
{
    public class Math
    {
        //PI. Used to convert angles
        public const float PI = 3.14159265359f;

        //Converts an angle to radian
        public const float ToRadians = 0.01745329251f;
        //converts a raidan to a degree
        public const float ToDegrees = 57.2957795131f;

        //Returns the absolute value. -3 would then return 3
        public static float Abs(float value)
        {
            if (value < 0)
                return value * -1f;

            return value;
        }

        //Returns the absolute value. -3 would then return 3
        public static int Abs(int value)
        {
            if (value < 0)
                return value * -1;

            return value;
        }

        //Returns the highest value
        public static int Max(int a, int b)
        {
            return a > b ? a : b;
        }

        //Clamps the value to the min or max value. if min is 0 and value is -1, then value is 0. If max is 10 and value is 11, value is 10
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
        
        //Clamps the value to the min or max value. if min is 0 and value is -1, then value is 0. If max is 10 and value is 11, value is 10
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

        //Properly rounds a float to an int. 3.2 would be 3 and 3.7 would be 4
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