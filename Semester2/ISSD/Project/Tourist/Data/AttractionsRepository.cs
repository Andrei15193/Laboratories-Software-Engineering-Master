using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Tourist.Models;
using Windows.ApplicationModel;
using Windows.Storage;
namespace Tourist.Data
{
    public class AttractionsRepository
    {
        private readonly List<Attraction> _attractions;
        private readonly IDictionary<string, ICollection<Comment>> _commentsByAttractionTitle;
        private readonly List<Attraction> _attractionsToVisit;

        public AttractionsRepository()
        {
            _attractions = new List<Attraction>();
            _commentsByAttractionTitle = new Dictionary<string, ICollection<Comment>>(StringComparer.OrdinalIgnoreCase);

            _commentsByAttractionTitle["Mirror Street"] = new List<Comment>
                                                          {
                                                              new Comment
                                                              {
                                                                  Text = "Awesome buildings!",
                                                                  Author = "John",
                                                                  PostTime = DateTimeOffset.Now
                                                              },
                                                              new Comment
                                                              {
                                                                  Text = "Not just any mirror, a very long one! Recommended",
                                                                  Author = "Eliza",
                                                                  PostTime = DateTimeOffset.Now.AddDays(-1)
                                                              },
                                                              new Comment
                                                              {
                                                                  Text = "Go visit this, it's very cool!",
                                                                  Author = "Peter",
                                                                  PostTime = DateTimeOffset.Now.AddDays(-2)
                                                              }
                                                          };
            _attractionsToVisit = new List<Attraction>();
        }

        public Task<IEnumerable<Attraction>> GetAttractionsToVisitAsync()
        {
            return GetAttractionsToVisitAsync(CancellationToken.None);
        }
        public async Task<IEnumerable<Attraction>> GetAttractionsToVisitAsync(CancellationToken cancellationToken)
        {
            //await Task.Delay(2000, cancellationToken);
            await Task.Yield();

            return _attractionsToVisit;
        }

        public Task<Attraction> GetAttractionByAsync(string title)
        {
            return GetAttractionByAsync(title, CancellationToken.None);
        }
        public async Task<Attraction> GetAttractionByAsync(string title, CancellationToken cancellationToken)
        {
            //await Task.Delay(1000, cancellationToken);
            var attractions = await _GetAllAttractionsAsync(cancellationToken);

            return attractions.ToList().Find(attraction => attraction.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        public Task<IReadOnlyCollection<Comment>> GetAttractionCommentsAsync(string title)
        {
            return GetAttractionCommentsAsync(title, CancellationToken.None);
        }
        public async Task<IReadOnlyCollection<Comment>> GetAttractionCommentsAsync(string title, CancellationToken cancellationToken)
        {
            await Task.Yield();
            //await Task.Delay(3000, cancellationToken);

            ICollection<Comment> comments;
            if (_commentsByAttractionTitle.TryGetValue(title, out comments))
                return comments.OrderByDescending(comment => comment.PostTime).ToList();
            else
                return Enumerable.Empty<Comment>().ToList();
        }

        public Task AddCommentToAttractionAsync(string attractionTitle, Comment comment)
        {
            return AddCommentToAttractionAsync(attractionTitle, comment, CancellationToken.None);
        }
        public async Task AddCommentToAttractionAsync(string attractionTitle, Comment comment, CancellationToken cancellationToken)
        {
            await Task.Yield();

            ICollection<Comment> comments;
            if (_commentsByAttractionTitle.TryGetValue(attractionTitle, out comments))
                comments.Add(comment);
            else
                _commentsByAttractionTitle.Add(attractionTitle, new List<Comment> { comment });
        }

        public Task AddAttractionToVisitAsync(Attraction attraction)
        {
            return AddAttractionToVisitAsync(attraction, CancellationToken.None);
        }
        public async Task AddAttractionToVisitAsync(Attraction attraction, CancellationToken cancellationToken)
        {
            if (attraction == null)
                throw new ArgumentNullException("attraction");

            await Task.Yield();

            if (_attractionsToVisit.All(existingAttraction => !existingAttraction.Title.Equals(attraction.Title, StringComparison.OrdinalIgnoreCase)))
            {
                _attractionsToVisit.Add(attraction);
                OnAddedAttractionToVisit(new AttractionsToVisitEventArgs(attraction));
            }
        }
        public Task RemoveAttractionToVisitAsync(string attractionTitle)
        {
            return RemoveAttractionToVisitAsync(attractionTitle, CancellationToken.None);
        }
        public async Task RemoveAttractionToVisitAsync(string attractionTitle, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(attractionTitle))
                if (attractionTitle == null)
                    throw new ArgumentNullException("attractionTitle");
                else
                    throw new ArgumentException("Cannot be empty or white space!", "attractionTitle");

            await Task.Yield();

            var attraction = _attractionsToVisit
                            .FirstOrDefault(attractionToVisit => attractionToVisit
                                                                .Title
                                                                .Equals(attractionTitle,
                                                                        StringComparison.OrdinalIgnoreCase));
            if (attraction != null && _attractionsToVisit.Remove(attraction))
                OnRemovedAttractionToVisit(new AttractionsToVisitEventArgs(attraction));
        }

        public Task<IReadOnlyCollection<Attraction>> SearchForAttractionsAsync(IEnumerable<string> keywords, SearchOptions searchOptions)
        {
            return SearchForAttractionsAsync(keywords, searchOptions, CancellationToken.None);
        }
        public async Task<IReadOnlyCollection<Attraction>> SearchForAttractionsAsync(IEnumerable<string> keywords, SearchOptions searchOptions, CancellationToken cancellationToken)
        {
            if (keywords == null)
                throw new ArgumentNullException("keywords");

            //await Task.Delay(4000, cancellationToken);

            keywords = keywords.Select(keyword => keyword.ToUpper()).ToList();
            var attractions = await _GetAllAttractionsAsync(cancellationToken);

            return attractions.Where(attraction =>
                                     {
                                         var title = (searchOptions.SearchTitle ? attraction.Title.ToUpper() : attraction.Title);
                                         var description = (searchOptions.SearchTitle ? attraction.Description.ToUpper() : attraction.Description);

                                         return (searchOptions.SearchTitle
                                                 && keywords.Any(keyword => title.Contains(keyword)))
                                             || (searchOptions.SearchTags
                                                 && attraction.Tags
                                                              .Select(tag => tag.ToUpper())
                                                              .Any(tag => keywords.Any(keyword => tag.Contains(keyword))))
                                             || (searchOptions.SearchDescription
                                                 && keywords.Any(keyword => description.Contains(keyword)));
                                     })
                              .ToList();
        }

        public Task AddAttractionAsync(Attraction attraction)
        {
            return AddAttractionAsync(attraction, CancellationToken.None);
        }
        public async Task AddAttractionAsync(Attraction attraction, CancellationToken cancellationToken)
        {
            await Task.Yield();

            if (attraction.ContactDetails != null
                && string.IsNullOrWhiteSpace(attraction.ContactDetails.Address)
                && string.IsNullOrWhiteSpace(attraction.ContactDetails.PhoneNumber)
                && attraction.ContactDetails.Website == null)
                attraction.ContactDetails = null;

            var attractions = await _GetAllAttractionsAsync(cancellationToken);
            if (attractions.Any(existingAttraction => existingAttraction.Title.Equals(attraction.Title, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException("An attraction with the same name already exists");
            else
                _attractions.Add(attraction);
        }

        public event EventHandler<AttractionsToVisitEventArgs> AddedAttractionToVisit;
        protected virtual void OnAddedAttractionToVisit(AttractionsToVisitEventArgs eventArgs)
        {
            EventHandler<AttractionsToVisitEventArgs> eventHandler = AddedAttractionToVisit;
            if (eventHandler != null)
                eventHandler(this, eventArgs);
        }
        public event EventHandler<AttractionsToVisitEventArgs> RemovedAttractionToVisit;
        protected virtual void OnRemovedAttractionToVisit(AttractionsToVisitEventArgs eventArgs)
        {
            EventHandler<AttractionsToVisitEventArgs> eventHandler = RemovedAttractionToVisit;
            if (eventHandler != null)
                eventHandler(this, eventArgs);
        }

        private async Task<IEnumerable<Attraction>> _GetAllAttractionsAsync(CancellationToken cancellationToken)
        {
            var serializer = new DataContractSerializer(typeof(Data));
            var file = await Package.Current.InstalledLocation.GetFileAsync(@"Assets\MockData.xml").AsTask(cancellationToken);

            using (var fileRandomAccessStream = await file.OpenAsync(FileAccessMode.Read).AsTask(cancellationToken))
            using (var fileStream = fileRandomAccessStream.AsStream())
                return ((Data)serializer.ReadObject(fileStream)).Attractions.Concat(_attractions);
        }
    }

    [DataContract(Namespace = Constants.XmlNamespace)]
    public class Data
    {
        private AttractionList _attractions;
        [DataMember]
        public AttractionList Attractions
        {
            get
            {
                if (_attractions == null)
                    _attractions = new AttractionList();

                return _attractions;
            }
        }

    }
    [CollectionDataContract(Name = "Attractions", ItemName = "Attraction", Namespace = Constants.XmlNamespace)]
    public class AttractionList
        : List<Attraction>
    {
    }
}