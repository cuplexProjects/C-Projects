using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SecureMemo.TextSearchModels;

namespace SecureMemo.Services
{
    public class TabSearchEngineService: IDisposable
    {
        private static TabSearchEngineService _instance;
        private List<SearchCollectionItem> _searchCollection;
        private TextSearchState _previouSearchState;
        private TextSearchState _searchState;

        private TabSearchEngineService()
        {
            _searchCollection = new List<SearchCollectionItem>();
        }

        public List<TextSearchResult> FindTextInAllTabs(TextSearchProperties searchProperties)
        {
            if (_searchState == null)
            {
                _searchState = new TextSearchState
                {
                    InitialTabIndex = searchProperties.TabIndex,
                    TabIndex = searchProperties.TabIndex,
                    StartPos = searchProperties.StartPosition,
                    InitialStartPos = searchProperties.StartPosition,
                    MatchesFound = 0
                };
            }

            TextSearchResultCollection resultCollection= new TextSearchResultCollection();
            int searchStrLength = searchProperties.SearchText.Length;

            int tabPageIndex = searchProperties.TabIndex;

            for (int i = 0; i < _searchCollection.Count; i++)
            {
                if (tabPageIndex > _searchCollection.Count)
                    tabPageIndex = 0;

                var item = _searchCollection.FirstOrDefault(x => x.TabPageIndex == tabPageIndex);

                if (item != null)
                {

                    int index = -1;

                    while (index > 0)
                    {
                        if (_searchState.StartPos > item.RichTextBox.TextLength)
                            break;

                        index = item.RichTextBox.Find(searchProperties.SearchText, _searchState.StartPos, RichTextBoxFinds.None);

                        if (index == -1)
                            break;

                        TextSearchResult searchResult = new TextSearchResult { StartPos = index, Length = searchStrLength, TabIndex = tabPageIndex };
                        resultCollection.AddSearchResult(searchResult);

                        _searchState.StartPos = index + searchStrLength;
                    }

                }

                _searchState.StartPos = 0;
                tabPageIndex++;
            }


            return resultCollection.GetAllSearchResults();

        }

        //private void RestSearchDataParameters(bool rebuildSearchCollection)
        //{
        //    if (rebuildSearchCollection)
        //        _textSearchItems = null;

        //    if (_textSearchItems == null)
        //    {
        //        _textSearchItems = new List<SearchCollectionItem>();
        //        int index = 0;

        //        foreach (TabPage tabPage in tabControlNotepad.TabPages)
        //        {
        //            var memoTabPageControl = tabPage.Controls[0] as MemoTabPageControl;
        //            if (memoTabPageControl == null) return;
        //            var richTextBox = ControlHelper.GetChildControlByName(memoTabPageControl, memoTabPageControl.TabPageControlTextboxName) as RichTextBox;
        //            _textSearchItems.Add(new SearchCollectionItem { RichTextBox = richTextBox, TabPageIndex = index++ });
        //        }
        //    }

        //    _searchState = new TextSearchState { TabIndex = tabControlNotepad.SelectedIndex, EndPos = -1, FirstItteration = true };
        //}





        public void Dispose()
        {
            _instance = null;
        }

        public static TabSearchEngineService Instance => _instance ?? (_instance = new TabSearchEngineService());
    }
}
