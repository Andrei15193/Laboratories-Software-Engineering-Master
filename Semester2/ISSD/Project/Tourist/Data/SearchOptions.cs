using System;
namespace Tourist.Data
{
    public struct SearchOptions
        : IEquatable<SearchOptions>
    {
        public static bool operator ==(SearchOptions left, SearchOptions right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(SearchOptions left, SearchOptions right)
        {
            return !left.Equals(right);
        }

        public static bool operator ==(SearchOptions left, IEquatable<SearchOptions> right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(SearchOptions left, IEquatable<SearchOptions> right)
        {
            return !left.Equals(right);
        }
        public static bool operator ==(IEquatable<SearchOptions> left, SearchOptions right)
        {
            return right.Equals(left);
        }
        public static bool operator !=(IEquatable<SearchOptions> left, SearchOptions right)
        {
            return !right.Equals(left);
        }

        public static bool operator ==(SearchOptions left, object right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(SearchOptions left, object right)
        {
            return !left.Equals(right);
        }
        public static bool operator ==(object left, SearchOptions right)
        {
            return right.Equals(left);
        }
        public static bool operator !=(object left, SearchOptions right)
        {
            return !right.Equals(left);
        }

        private readonly bool _ignoreTitle;
        private readonly bool _ignoreTags;
        private readonly bool _searchDescription;

        public SearchOptions(bool searchTitle, bool searchTags, bool searchDescription)
        {
            _ignoreTitle = !searchTitle;
            _ignoreTags = !searchTags;
            _searchDescription = searchDescription;
        }

        public bool SearchTitle
        {
            get
            {
                return !_ignoreTitle;
            }
        }
        public bool SearchTags
        {
            get
            {
                return !_ignoreTags;
            }
        }
        public bool SearchDescription
        {
            get
            {
                return _searchDescription;
            }
        }

        public bool Equals(SearchOptions other)
        {
            return (SearchTitle == other.SearchTitle
                    && SearchTags == other.SearchTags
                    && SearchDescription == other.SearchDescription);
        }
        public override bool Equals(object obj)
        {
            return (obj is SearchOptions && Equals((SearchOptions)obj));
        }
        public override int GetHashCode()
        {
            return (SearchTitle.GetHashCode()
                    ^ SearchTags.GetHashCode()
                    ^ SearchDescription.GetHashCode());
        }
    }
}