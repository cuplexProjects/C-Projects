using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SecureMemo.DataModels;

namespace SecureMemo
{
    public partial class FormTabEdit : Form
    {
        private const int MAX_LABEL_LENGTH = 20;
        private const int MIN_LABEL_LENGTH = 1;
        private readonly TabPageDataCollection _tabPageDataCollection;
        private readonly List<DragableListItem> _listViewDataSource;

        public FormTabEdit(TabPageDataCollection tabPageDataCollection)
        {
            _tabPageDataCollection = tabPageDataCollection;
            VerifyAndCorrectIndexing(_tabPageDataCollection);
            _listViewDataSource = tabPageDataCollection.TabPageDictionary.Select(x => x.Value).Select(x => new DragableListItem {Index = x.PageIndex, Label = x.TabPageLabel, PageData = x}).ToList();
            InitializeComponent();
        }

        public bool TabDataChanged { get; private set; }

        private void VerifyAndCorrectIndexing(TabPageDataCollection tabPageDataCollection)
        {
            var pageIndexList = tabPageDataCollection.TabPageDictionary.Values.Select(x => x.PageIndex).ToList();
            bool incorrectPageIndexFound = pageIndexList.Any(pageIndex => pageIndexList.Count(k => k == pageIndex) > 1);
            if (!incorrectPageIndexFound) return;

            foreach (int key in tabPageDataCollection.TabPageDictionary.Keys)
            {
                tabPageDataCollection.TabPageDictionary[key].PageIndex = key;
            }

            TabDataChanged = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            _tabPageDataCollection.TabPageDictionary.Clear();
            foreach (DragableListItem dragableListItem in _listViewDataSource)
            {
                int index = dragableListItem.Index;
                _tabPageDataCollection.TabPageDictionary[index] = dragableListItem.PageData;
                _tabPageDataCollection.TabPageDictionary[index].PageIndex = dragableListItem.Index;
                _tabPageDataCollection.TabPageDictionary[index].TabPageLabel = dragableListItem.Label;
            }

            int lastIndex = _tabPageDataCollection.TabPageDictionary.Values.Max(x => x.PageIndex);
            if (_tabPageDataCollection.ActiveTabIndex > lastIndex)
                _tabPageDataCollection.ActiveTabIndex = lastIndex;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void frmTabEdit_Load(object sender, EventArgs e)
        {
            if (_tabPageDataCollection != null)
                LoadTabPageCollection();
        }

        private void LoadTabPageCollection()
        {
            listViewTabs.Clear();
            foreach (DragableListItem dragableListItem in _listViewDataSource)
                listViewTabs.Items.Add(new ListViewItem {Text = dragableListItem.Label, ImageIndex = 0});
        }

        private void listViewTabs_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label.Length <= MAX_LABEL_LENGTH && e.Label.Length >= MIN_LABEL_LENGTH)
            {
                DragableListItem listViewItem = _listViewDataSource.FirstOrDefault(x => x.Index == e.Item);
                if (listViewItem != null) listViewItem.Label = e.Label;
                TabDataChanged = true;
                return;
            }
            string textboxMessage = $"Invalid label length. The tab label must be between {MIN_LABEL_LENGTH} and {MAX_LABEL_LENGTH} characters long";
            e.CancelEdit = true;
            MessageBox.Show(this, textboxMessage, "Unable to edit label", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void listViewTabs_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var listViewItem = e.Item as ListViewItem;
            if (listViewItem != null)
            {
                DragableListItem dragableListItem = _listViewDataSource.FirstOrDefault(x => x.Index == listViewItem.Index);
                if (dragableListItem != null)
                    DoDragDrop(dragableListItem, DragDropEffects.Move);
            }
        }

        private void listViewTabs_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void listViewTabs_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof (DragableListItem))) return;
            if (listViewTabs.SelectedItems.Count == 0)
                return;

            Point p = listViewTabs.PointToClient(MousePosition);
            ListViewItem liteViewItemClosestToDropPosition = GetClosestItemInRelationToDropPosition(listViewTabs, p);
            var dragItem = e.Data.GetData(typeof (DragableListItem)) as DragableListItem;

            if (liteViewItemClosestToDropPosition == null || liteViewItemClosestToDropPosition.Index == dragItem.Index)
                return;

            int originalIndex = dragItem.Index;
            int newIndex = liteViewItemClosestToDropPosition.Index;

            if (radioButtonSwitch.Checked)
            {
                DragableListItem listItem1 = _listViewDataSource.First(x => x.Index == newIndex);
                DragableListItem listItem2 = _listViewDataSource.First(x => x.Index == originalIndex);

                listItem1.Index = originalIndex;
                listItem2.Index = newIndex;
            }
            else
            {
                int minIndex = Math.Min(newIndex, originalIndex);
                int maxIndex = Math.Max(newIndex, originalIndex);
                bool leftShift = newIndex > originalIndex;

                if (leftShift)
                {
                    var itemsToShiftIndex = _listViewDataSource.Where(x => x.Index > minIndex && x.Index <= maxIndex).OrderBy(x => x.Index).ToList();
                    DragableListItem listViewItemToSwitch = _listViewDataSource.First(x => x.Index == minIndex);

                    foreach (DragableListItem listItem in itemsToShiftIndex)
                        listItem.Index = listItem.Index - 1;

                    listViewItemToSwitch.Index = maxIndex;
                }
                else
                {
                    var itemsToShiftIndex = _listViewDataSource.Where(x => x.Index >= minIndex && x.Index < maxIndex).OrderBy(x => x.Index).ToList();
                    DragableListItem listViewItemToSwitch = _listViewDataSource.First(x => x.Index == maxIndex);

                    foreach (DragableListItem listItem in itemsToShiftIndex)
                        listItem.Index = listItem.Index + 1;

                    listViewItemToSwitch.Index = minIndex;
                }
            }

            TabDataChanged = true;
            _listViewDataSource.Sort();
            LoadTabPageCollection();
        }

        private ListViewItem GetClosestItemInRelationToDropPosition(ListView listView, Point p)
        {
            var allListViewItems = (from ListViewItem viewItem in listView.Items select viewItem).ToList();

            int xMax = allListViewItems.Max(x => x.Bounds.Right);
            int yMax = allListViewItems.Max(x => x.Bounds.Bottom);
            int xMin = allListViewItems.Min(x => x.Bounds.Left);
            int yMin = allListViewItems.Min(x => x.Bounds.Top);

            if (p.X > xMax)
                p.X = xMax;

            if (p.X < xMin)
                p.X = xMin;

            if (p.Y > yMax)
                p.Y = yMax;

            if (p.Y < yMin)
                p.Y = yMin;

            var allListViewItemsOnTheSameHorizontalLevel = allListViewItems.Where(x => x.Bounds.Top <= p.Y && x.Bounds.Bottom >= p.Y).ToList();
            return allListViewItemsOnTheSameHorizontalLevel.FirstOrDefault(x => x.Bounds.Left <= p.X && x.Bounds.Right >= p.X);
        }

        private void listViewTabs_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof (DragableListItem)))
                e.Effect = e.AllowedEffect;
        }

        private void addNewTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int newIndex = _listViewDataSource.Max(x => x.Index) + 1;
            string tabPageLabel = "Page" + (newIndex + 1);
            var tabPageData = new TabPageData {PageIndex = newIndex, TabPageLabel = tabPageLabel, TabPageText = ""};
            tabPageData.GenerateUniqueIdIfNoneExists();
            _listViewDataSource.Add(new DragableListItem {Index = newIndex, Label = tabPageLabel, PageData = tabPageData});

            TabDataChanged = true;
            LoadTabPageCollection();
        }

        private void deleteSelectedTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewTabs.SelectedItems.Count <= 0) return;
            ListViewItem itemToDelete = listViewTabs.SelectedItems[0];
            _listViewDataSource.RemoveAll(x => x.Index == itemToDelete.Index);

            //Reindex collection
            int pageIndex = 0;
            foreach (DragableListItem dragableListItem in _listViewDataSource)
            {
                dragableListItem.Index = pageIndex;
                pageIndex++;
            }

            TabDataChanged = true;
            LoadTabPageCollection();
        }

        private void contextMenu_Opening(object sender, CancelEventArgs e)
        {
            deleteSelectedTabToolStripMenuItem.Enabled = listViewTabs.SelectedItems.Count > 0;
        }

        private void FormTabEdit_ResizeBegin(object sender, EventArgs e)
        {
        }

        private void FormTabEdit_ResizeEnd(object sender, EventArgs e)
        {
            LoadTabPageCollection();
            listViewTabs.Refresh();
        }

        private class DragableListItem : IComparable<DragableListItem>
        {
            public int Index { get; set; }
            public string Label { get; set; }
            public TabPageData PageData { get; set; }

            public int CompareTo(DragableListItem other)
            {
                return Index.CompareTo(other.Index);
            }

            public override string ToString()
            {
                return $"Label: {Label}, Index: {Index}";
            }
        }
    }
}