using System.Diagnostics;

namespace NekinuSoft
{
    public class Time
    {
        //The time of the program
        private Stopwatch time;

        private float t;
        //last updated deltaTime
        private long last_time;
        //last updated fpsTime
        private long last_fps_time;

        //the amount of frames in the second
        private int frames;

        //the time since the last frame
        public static float deltaTime { get; private set; }
        //The amount of frames in a second
        public static int FPS { get; private set; }

        private static float time_scale = 1;

        /// <summary>
        /// How fast the game will run at. Base is 1
        /// </summary>
        public static float TimeScale
        {
            get => time_scale;
            set => time_scale = value;
        }

        //Constructor
        public Time()
        {
            t = 0;
            time = new Stopwatch();
            //Starts the timer
            time.Start();
        }
        
        public void Update()
        {
            //increase the frame count
            frames++;

            //how many milliseconds since the game started
            long now = time.ElapsedMilliseconds;

            //if the current millisecond count is greater than the last updated fps time + 1000 milliseconds (1 second), then
            if (now > last_fps_time + 1000)
            {
                //sets the public frame count
                FPS = frames;
                //resets the frame count
                frames = 0;
                //sets the last updated fps time to the current
                last_fps_time = now;
            }

            //sets the deltaTime
            deltaTime = ((now - last_time) / 1000f);

            Console.WriteLine($"FPS {FPS} Time running {t} Delta time: {deltaTime}");
            t += 1 * deltaTime;
            last_time = now;
        }
    }
}