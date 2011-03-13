namespace URLExpander.ViewModels
{
    using System.Linq;

    public class GooglViewModel : ExpandedUrlViewModelBase
    {
        private readonly GooglUrlExpander _googlUrlExpander;
        private readonly string _shortUrl;

        private bool _expanded;
        private string _url;
        private int? _globalClicks;
        private int? _userClicks;

        public GooglViewModel(GooglUrlExpander googlUrlExpander, string shortUrl)
        {
            _googlUrlExpander = googlUrlExpander;
            _shortUrl = shortUrl;
        }

        public override int? UserClicks
        {
            get
            {
                if (!_expanded)
                {
                    _expanded = true;
                    ExpandUrl();
                }
                return _userClicks;
            }
            set
            {
                _userClicks = value ?? 0;
                OnPropertyChanged("UserClicks");
                OnPropertyChanged("NumberOfClicksText");
            }
        }

        public override int? GlobalClicks
        {
            get
            {
                if (!_expanded)
                {
                    ExpandUrl();
                }
                return _globalClicks;
            }
            set
            {
                _globalClicks = value ?? 0;
                OnPropertyChanged("GlobalClicks");
                OnPropertyChanged("NumberOfClicksText");
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
            _googlUrlExpander.ExpandUrl(
                _shortUrl, 
                result =>
                {
                    Url = result.LongUrl;
                    if (result.Analytics == null || result.Analytics.AllTime == null)
                    {
                        return;
                    }

                    GlobalClicks = result.Analytics.AllTime.LongUrlClicks;
                    UserClicks = result.Analytics.AllTime.ShortUrlClicks;
                });
        }
    }
}