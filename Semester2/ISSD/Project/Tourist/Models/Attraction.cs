using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
namespace Tourist.Models
{
    [DataContract(Namespace = Constants.XmlNamespace)]
    public class Attraction
        : ValidatableDataModel
    {
        private string _title;
        private string _description;
        private float _rating;
        private ContactDetails _contatDetails;

        public Attraction()
        {
            _Initialize(default(StreamingContext));
        }

        [DataMember]
        public bool Visited
        {
            get;
            set;
        }

        [DataMember]
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                Assert(!string.IsNullOrEmpty(_title), "Title cannot be empty");
            }
        }

        [DataMember]
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                Assert(!string.IsNullOrEmpty(_description), "Description cannot be empty");
            }
        }

        [DataMember]
        public XmlUri ImageUri
        {
            get;
            set;
        }

        [DataMember]
        public Coordinates Coordinates
        {
            get;
            private set;
        }

        [DataMember]
        public float Rating
        {
            get
            {
                return _rating;
            }
            set
            {
                _rating = value;
                Assert(0 <= _rating && _rating <= 5, "Rating must be between 0 and 5");
            }
        }

        [DataMember]
        public ContactDetails ContactDetails
        {
            get
            {
                return _contatDetails;
            }
            set
            {
                if (_contatDetails != null)
                    UnsubscribeValidatableProperty();

                _contatDetails = value;

                if (_contatDetails != null)
                    SubscribeValidatableProperty(_contatDetails);
            }
        }

        [DataMember]
        public TagCollection Tags
        {
            get;
            private set;
        }

        [DataMember(Name = "Pictures")]
        public PictureUriCollection PictureUris
        {
            get;
            private set;
        }

        [OnDeserializing]
        private void _Initialize(StreamingContext streamingContext)
        {
            Visited = false;
            Title = null;
            Description = null;
            ImageUri = null;
            Coordinates = new Coordinates();
            Rating = 0F;
            ContactDetails = null;
            Tags = new TagCollection(this);
            PictureUris = new PictureUriCollection();
        }

        [CollectionDataContract(Name = "Tags", ItemName = "Tag", Namespace = Constants.XmlNamespace)]
        public class TagCollection
            : ICollection<string>
        {
            private readonly Attraction _attraction;
            private readonly ICollection<string> _tags;

            internal TagCollection(Attraction attraction)
            {
                if (attraction == null)
                    throw new ArgumentNullException("attraction");

                _attraction = attraction;
                _tags = new List<string>();

                _Validate();
            }
            private TagCollection()
            {
                _attraction = null;
                _tags = new List<string>();
            }

            public void Add(string tag)
            {
                if (string.IsNullOrWhiteSpace(tag))
                    if (tag == null)
                        throw new ArgumentNullException("tag");
                    else
                        throw new ArgumentException("Cannot be empty or white space!", "tag");
                if (!Contains(tag))
                {
                    _tags.Add(tag);
                    _Validate();
                }
            }

            public void Clear()
            {
                _tags.Clear();
                _Validate();
            }

            public bool Contains(string tag)
            {
                return (!string.IsNullOrWhiteSpace(tag) && _tags.Contains(tag, StringComparer.OrdinalIgnoreCase));
            }

            public void CopyTo(string[] array, int arrayIndex)
            {
                _tags.CopyTo(array, arrayIndex);
            }

            public int Count
            {
                get
                {
                    return _tags.Count;
                }
            }

            bool ICollection<string>.IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            public bool Remove(string tag)
            {
                if (!string.IsNullOrWhiteSpace(tag) && _tags.Remove(tag))
                {
                    _Validate();
                    return true;
                }
                else
                    return false;
            }

            public IEnumerator<string> GetEnumerator()
            {
                return _tags.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private void _Validate()
            {
                if (_attraction != null)
                    _attraction.Assert(_tags.Count > 0, "Attractions must have at least one tag", "Tags");
            }
        }

        [CollectionDataContract(Name = "Pictures", ItemName = "Uri", Namespace = Constants.XmlNamespace)]
        public class PictureUriCollection
            : List<XmlUri>
        {
        }
    }
}