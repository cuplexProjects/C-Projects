using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageView.Models.UserInteraction
{
    public class UserInteractionInformation : UserInteractionBase
    {
        public event EventHandler OnQueryCompleted;

        public void Execute()
        {
            OnQueryCompleted?.Invoke(this, new EventArgs());
        }
    }
}
