using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageView.Library.EventHandlers;

namespace ImageView.Interfaces
{
    public interface IExceptionEventHandler
    {
        event ExceptionEventHandler OnUnexpectedException;
    }
}
