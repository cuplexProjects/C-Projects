using System;
using System.Windows.Forms;

namespace SecureMemo.UserControls
{
    public partial class MemoTabPageControl : UserControl
    {
        public MemoTabPageControl(string memoTabName, int index)
        {
            InitializeComponent();

            TabNameIndex = index;
            Name = memoTabName + TabNameIndex;
            TabPageControlTextboxName = "RichTextBox" + TabNameIndex;
            tabRichTextBox.Name = TabPageControlTextboxName;
        }

        public string TabPageControlTextboxName { get; }

        public int TabNameIndex { get; }

        public event EventHandler SelectionChanged;

        public event EventHandler TabTextDataChanged;

        private void tabRichTextBox_TextChanged(object sender, EventArgs e)
        {
            TabTextDataChanged?.Invoke(this, new EventArgs());
        }

        private void tabRichTextBox_SelectionChanged(object sender, EventArgs e)
        {
            if (SelectionChanged != null)
                SelectionChanged.Invoke(sender, e);
        }
    }
}