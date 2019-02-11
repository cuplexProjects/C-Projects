using System;
using SecureMemo.DataModels;

namespace SecureMemo.TextSearchModels
{
    public class TabSearchEngine
    {
        private readonly TabPageDataCollection _tabPageDataCollection;

        private TextSearchState _searchState;

        public TabSearchEngine(TabPageDataCollection tabPageDataCollection, int textStartIndex = 0)
        {
            _searchState = new TextSearchState();
            _tabPageDataCollection = tabPageDataCollection;
            ResetSearchState(_tabPageDataCollection.ActiveTabIndex, textStartIndex, 0);
        }

        public bool SelectionSetByCode { get; set; }

        public void ResetSearchState(int tabIndex, int textStartPos, int selectionLength)
        {
            _searchState = new TextSearchState
            {
                TabIndex = tabIndex,
                StartPosUp = textStartPos,
                StartPosDown = textStartPos + selectionLength,
                InitialTabIndex = tabIndex,
                InitialStartPos = textStartPos,
                TabCount = _tabPageDataCollection.TabPageDictionary.Count
            };
        }

        public TextSearchResult GetTextSearchResult(TextSearchProperties searchProperties)
        {
            var textSearchResult = new TextSearchResult();

            if (searchProperties.SearchAllTabs)
                while (_searchState.TabPageSearchCount <= _searchState.TabCount)
                {
                    textSearchResult = GetTextSearchResultInActiveState(searchProperties);
                    if (textSearchResult.SearchTextFound)
                    {
                        textSearchResult.TabIndex = _searchState.TabIndex;
                        break;
                    }

                    if (_searchState.TabIndex == _searchState.InitialTabIndex)
                        break;

                    _searchState.TabPageSearchCount++;
                }
            else
                textSearchResult = GetTextSearchResultInActiveState(searchProperties);

            return textSearchResult;
        }

        private TextSearchResult GetTextSearchResultInActiveState(TextSearchProperties searchProperties)
        {
            var textSearchResult = new TextSearchResult();

            TabPageData tabPageData = _tabPageDataCollection.TabPageDictionary[_searchState.TabIndex];
            int matchPos;

            if (string.IsNullOrEmpty(tabPageData.TabPageText))
            {
                SetNextTabIndex(searchProperties.SearchDirection);
                return textSearchResult;
            }

            if (searchProperties.SearchDirection == TextSearchEvents.SearchDirection.Down)
            {
                if (_searchState.StartPosDown >= tabPageData.TabPageText.Length)
                    _searchState.StartPosDown = tabPageData.TabPageText.Length - 1;
                else if (_searchState.StartPosDown < 0)
                    _searchState.StartPosDown = 0;

                matchPos = tabPageData.TabPageText.IndexOf(searchProperties.SearchText, _searchState.StartPosDown,
                    searchProperties.CaseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase);
            }
            else
            {
                if (_searchState.StartPosUp < 0 || _searchState.StartPosUp > tabPageData.TabPageText.Length)
                    _searchState.StartPosUp = tabPageData.TabPageText.Length;

                matchPos = tabPageData.TabPageText.LastIndexOf(searchProperties.SearchText, _searchState.StartPosUp,
                    searchProperties.CaseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase);
            }

            if (matchPos >= 0)
            {
                _searchState.StartPosDown = matchPos + searchProperties.SearchText.Length;
                _searchState.StartPosUp = matchPos - searchProperties.SearchText.Length;

                if (_searchState.StartPosUp < 0)
                    _searchState.StartPosUp = 0;

                textSearchResult.StartPos = matchPos;
                textSearchResult.Length = searchProperties.SearchText.Length;
                _searchState.MatchesFound++;
                textSearchResult.SearchTextFound = true;
            }
            else
            {
                _searchState.StartPosDown = 0;
                _searchState.StartPosUp = -1;
                SetNextTabIndex(searchProperties.SearchDirection);
                textSearchResult.SearchTextFound = false;
            }

            return textSearchResult;
        }

        private void SetNextTabIndex(TextSearchEvents.SearchDirection searchDirection)
        {
            if (searchDirection == TextSearchEvents.SearchDirection.Down)
                _searchState.TabIndex++;
            else
                _searchState.TabIndex--;

            if (_searchState.TabIndex >= _tabPageDataCollection.TabPageDictionary.Count)
                _searchState.TabIndex = 0;
            else if (_searchState.TabIndex < 0)
                _searchState.TabIndex = _tabPageDataCollection.TabPageDictionary.Count - 1;
        }
    }
}