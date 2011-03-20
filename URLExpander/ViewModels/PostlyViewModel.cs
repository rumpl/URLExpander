using URLExpander.UrlExpanders;

namespace URLExpander.ViewModels
{
    public class PostlyViewModel : ExpandedUrlViewModelBase
    {
        private readonly PostlyUrlExpander _postlyUrlExpander;
        private readonly string _shortUrl;

        private bool _expanded;
        private string _url;

        public PostlyViewModel(PostlyUrlExpander postlyUrlExpander, string shortUrl)
        {
            _postlyUrlExpander = postlyUrlExpander;
            _shortUrl = shortUrl;
        }

        public override int? UserClicks
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public override int? GlobalClicks
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public override string Url
        {
            get
            {
                if (!_expanded)
                {
                    ExpandUrl();
                    _expanded = true;
                }
                return string.IsNullOrWhiteSpace(_url) ? "Searching..." : _url;
            }
            set
            {
                _url = value;
                OnPropertyChanged("Url");
            }
        }

        private void ExpandUrl()
        {
            _postlyUrlExpander.ExpandUrl(
                _shortUrl,
                result =>
                {
                    if (result == null || result.Post == null) return;
                    Url = result.Post.Link;
                });
        }
    }
}
