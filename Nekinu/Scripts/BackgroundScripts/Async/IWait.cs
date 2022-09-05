namespace NekinuSoft
{
    //An interface for the async classes. Used to delay code on a separate thread
    public interface IWait
    {
        Task run();
    }
}