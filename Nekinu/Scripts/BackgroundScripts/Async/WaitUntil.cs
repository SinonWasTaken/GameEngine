namespace NekinuSoft
{
    public class WaitUntil : IWait
    {
        //A predicate statement that waits until something is true or false
        private Func<bool> predicate;
        
        //Constructor
        public WaitUntil(Func<bool> predicate)
        {
            this.predicate = predicate;
        }

        public async Task run()
        {
            //While the predicate has not been completed
            while (!predicate())
            {
                //Wait a 10th of a second
                await Task.Delay(100);
            }
        }
    }
}