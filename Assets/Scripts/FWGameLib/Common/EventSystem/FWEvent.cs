namespace FWGameLib.Common.EventSystem
{
    /// <summary>
    /// Base event class with handler
    /// </summary>
    /// <typeparam name="T">Event type</typeparam>
    public abstract class FWEvent<T> where T : FWEvent<T>
    {
        public delegate void OnEvent(T e);
        public static event OnEvent Handler;

        public void Invoke()
        {
            Handler?.Invoke((T) this);
        }
    }
}