using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageView.Models.UserInteraction
{
    public class UserQuestionEventArgs : EventArgs
    {
        public UserQuestionEventArgs(UserInteractionQuestion userQuestion)
        {
            UserQuestion = userQuestion;
        }

        public UserInteractionQuestion UserQuestion { get; }
    }
}
