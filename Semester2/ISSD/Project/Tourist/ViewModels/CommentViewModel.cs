using System;
using Tourist.Models;
namespace Tourist.ViewModels
{
    public class CommentViewModel
        : DataViewModel<Comment>
    {
        public CommentViewModel(Comment comment)
            : base(comment)
        {
            Author = GetPropertyViewModel("Author",
                                          () => DataModel.Author,
                                          value => DataModel.Author = value);
            Text = GetPropertyViewModel("Text",
                                        () => DataModel.Text,
                                        value => DataModel.Text = value);
        }

        public IDataPropertyViewModel<string> Author
        {
            get;
            private set;
        }
        public IDataPropertyViewModel<string> Text
        {
            get;
            private set;
        }
        public DateTimeOffset PostTime
        {
            get
            {
                return DataModel.PostTime;
            }
            set
            {
                DataModel.PostTime = value;
                OnPropertyChanged();
            }
        }
    }
}