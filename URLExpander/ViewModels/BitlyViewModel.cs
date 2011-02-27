using System;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Windows.Browser;
using Seesmic.Sdp.Utils;
using URLExpander.Models;

namespace URLExpander.ViewModels
{
  public class BitlyViewModel : ObservableObject
  {
    private const string Login = "HOW CAN I KEEP FROM PUTTING THIS INTO SOURCE CONTROL...?";
    private const string ApiKey = "IS THERE A WAY THAT I CAN STORE THIS IN A CONFIG FILE OR SOMETHING...";

    private const string BitlyExpand =
      "http://api.bit.ly/v3/expand?login=" + Login + "&apiKey=" + ApiKey + "&shortUrl={0}&format=json";

    private const string BitlyClicks =
      "http://api.bit.ly/v3/clicks?login=" + Login + "&apiKey=" + ApiKey + "&shortUrl={0}";

    private bool _expanded;
    private int _globalClicks;
    private bool _numberOfClicks;
    private string _url;
    private int _userClicks;

    public BitlyViewModel(string shortUrl)
    {
      Hash = shortUrl;
    }

    private string Hash { get; set; }

    public string NoC
    {
      get { return string.Format("{0} click{1} ({2} global)", UserClicks, UserClicks == 1 ? "" : "s", _globalClicks); }
    }

    public int UserClicks
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
      }
    }

    public string Url
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
      var client = new WebClient();
      var uriString = string.Format(BitlyExpand, HttpUtility.UrlEncode(Hash));

      client.OpenReadCompleted += OnExpandCompleted;

      client.OpenReadAsync(new Uri(uriString));
    }

    private void GetNumberOfClicks()
    {
      var client = new WebClient();
      var uriString = string.Format(BitlyClicks, HttpUtility.UrlEncode(Hash));

      client.OpenReadCompleted += OnClicksCompleted;
      client.OpenReadAsync(new Uri(uriString));
    }

    private void OnClicksCompleted(object sender, OpenReadCompletedEventArgs args)
    {
      if (args.Error != null) return;

      var serializer = new DataContractJsonSerializer(typeof (BitlyExpandResponse));
      var ret = serializer.ReadObject(args.Result) as BitlyExpandResponse;

      if (ret == null) return;

      var value = ret.Data.Clicks.FirstOrDefault();

      if (value == null) return;
      _globalClicks = value.GlobalClicks;
      UserClicks = value.UserClicks;
      this.OnPropertyChanged("NoC");
    }

    private void OnExpandCompleted(object sender, OpenReadCompletedEventArgs args)
    {
      if (args.Error != null) return;

      var serializer = new DataContractJsonSerializer(typeof (BitlyExpandResponse));
      var result = serializer.ReadObject(args.Result) as BitlyExpandResponse;

      if (result == null) return;

      var fullUrl = result.Data.Urls.FirstOrDefault();

      Url = result.StatusCode == 200 && fullUrl != null ? fullUrl.Url : Hash;
    }
  }
}