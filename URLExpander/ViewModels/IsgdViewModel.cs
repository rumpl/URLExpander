namespace URLExpander.ViewModels
{
    using System.Linq;

    public class IsgdViewModel : ExpandedUrlViewModelBase
    {
        private readonly IsgdUrlExpander _isgdUrlExpander;
        private readonly string _shortUrl;

        private bool _expanded;
        private string _url;

        public IsgdViewModel(IsgdUrlExpander isgdUrlExpander, string shortUrl)
        {
            _isgdUrlExpander = isgdUrlExpander;
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
            this._isgdUrlExpander.ExpandUrl(
                _shortUrl, 
                result =>
                {
                    Url = result.Url;
                });
        }
    }
}