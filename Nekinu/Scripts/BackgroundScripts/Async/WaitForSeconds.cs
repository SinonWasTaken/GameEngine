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
            //delays the code execution by waitSeconds. 1000 miliseconds = 1 second, waitSeconds = 5.5f then this would be waiting 5500 miliseconds
            await Task.Delay((int)(waitSeconds * 1000));
        }
    }
}