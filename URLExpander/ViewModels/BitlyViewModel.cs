using System.Linq;

namespace URLExpander.ViewModels
{
    public class BitlyViewModel : ExpandedUrlViewModelBase
    {
        private readonly BitlyUrlExpander _bitlyUrlExpander;
        private readonly string _shortUrl;

        private bool _expanded;
        private bool _numberOfClicks;
        private string _url;
        private int _globalClicks;
        private int _userClicks;

        public BitlyViewModel(BitlyUrlExpander bitlyUrlExpander, string shortUrl)
        {
            _bitlyUrlExpander = bitlyUrlExpander;
            _shortUrl = shortUrl;
        }

        public override int UserClicks
        {
            get
            {
                if (!_numberOfClicks)
                {
                    _numberOfClicks = true;
                    GetNumberOfClicks();
                }
                return _userClicks;
            }
            set
            {
                _userClicks = value;
                OnPropertyChanged("UserClicks");
                OnPropertyChanged("NumberOfClicksText");
            }
        }

        public override int GlobalClicks
        {
            get
            {
                if (!_numberOfClicks)
                {
                    _numberOfClicks = true;
                    GetNumberOfClicks();
                }
                return _globalClicks;
            }
            set
            {
                _globalClicks = value;
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
            _bitlyUrlExpander.ExpandUrl(
                _shortUrl, 
                result =>
                {
                    var expandedUrl = result.Data.Urls.FirstOrDefault();
                    Url = expandedUrl != null ? expandedUrl.Url : _shortUrl;
                });
        }

        private void GetNumberOfClicks()
        {
            _bitlyUrlExpander.GetNumberOfClicks(
                _shortUrl, 
                result =>
                {
                    var clicks = result.Data.Clicks.FirstOrDefault();
                    if (clicks == null)
                    {
                        return;
                    }

                    GlobalClicks = clicks.GlobalClicks;
                    UserClicks = clicks.UserClicks;
                });
        }
    }
}