namespace NekinuSoft
{
    public class WaitForSeconds : IWait
    {
        //The amount of seconds to stop the thread
        private float waitSeconds;
        
        //Constructor
        public WaitForSeconds(float seconds)
        {
            waitSeconds = seconds;
        }

        //Delays the program
        public async Task run()
        {
            //Waits for seconds * 1000 miliseconds = 1 second
            await Task.Delay((int)(waitSeconds * 1000));
        }
    }
}