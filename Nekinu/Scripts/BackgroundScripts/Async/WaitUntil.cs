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
            //Prevents the code from executing until a condition is met. I.E (5+i == 10). i is 4, the result is 9, meaning the code doesnt execute
            while (!predicate())
            {
                //Wait a tenth of a second
                await Task.Delay(100);
            }
        }
    }
}