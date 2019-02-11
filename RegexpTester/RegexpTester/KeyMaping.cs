using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RegexpTester
{
    public class KeyMaping
    {        
        public KeyMaping()
        {
        
        }
        public bool CtrlPressed { get; set; }
        public bool AltPressed { get; set; }
        public bool ShiftPressed { get; set; }
        public int KeyValue { get; set; }
    }   
}
