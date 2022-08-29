using System.Diagnostics;

namespace NekinuSoft
{
    public class Time
    {
        private Stopwatch time;

        private long last_time;
        private long last_fps_time;

        private int frames;

        public static float deltaTime { get; private set; }
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

        public Time()
        {
            time = new Stopwatch();
            time.Start();
        }

        public void Update()
        {
            frames++;

            long now = time.ElapsedMilliseconds;

            if (now > last_fps_time + 1000)
            {
                FPS = frames;
                frames = 0;
                last_fps_time = now;
            }

            deltaTime = ((now - last_time) / 1000f);

            last_time = now;
        }
    }
}