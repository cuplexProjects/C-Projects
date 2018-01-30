using ImageView.Library.EventHandlers;

namespace ImageView.Interfaces
{
    public interface IExceptionEventHandler
    {
        event ExceptionEventHandler OnUnexpectedException;
    }
}
