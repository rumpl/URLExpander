namespace URLExpander
{
    using System;
    using System.Runtime.Serialization.Json;
    using System.Windows.Browser;

    using URLExpander.Models;
    using URLExpander.ViewModels;

    ////[System.ComponentModel.Composition.Export(typeof(IUrlExpander))]
    public class GooglUrlExpander : UrlExpanderBase
    {
        private const string ApiKey = "Get your own API key: http://code.google.com/apis/urlshortener/v1/authentication.html#key";

        private static readonly Uri GooglApiBaseUri = new Uri("https://www.googleapis.com/urlshortener/v1/");

        private static readonly DataContractJsonSerializer ExpandResponseDeserializer = new DataContractJsonSerializer(typeof(GooglExpandResponse));

        public override void IfCanExpand(Uri uri, Action<IExpandedUrlViewModel> callback)
        {
            if (uri.DnsSafeHost != "goo.gl")
            {
                return;
            }

            this.DoCallback(uri, callback);
        }

        public void ExpandUrl(string shortUrl, Action<GooglExpandResponse> callback)
        {
            this.MakeGooglWebRequestAsync(
                ExpandResponseDeserializer,
                string.Format("url?shortUrl={0}", HttpUtility.UrlEncode(shortUrl)),
                callback);
        }

        private void MakeGooglWebRequestAsync(DataContractJsonSerializer responseDeserializer, string relativeUrl, Action<GooglExpandResponse> webRequestCallback)
        {
            this.MakeWebRequestAsync(
                responseDeserializer,
                new Uri(
                    GooglApiBaseUri,
                    relativeUrl + string.Format("&key={0}&projection=ANALYTICS_CLICKS", HttpUtility.UrlEncode(ApiKey))),
                webRequestCallback);
        }

        private void DoCallback(Uri uri, Action<IExpandedUrlViewModel> callback)
        {
            if (callback != null)
            {
                callback(new GooglViewModel(this, uri.AbsoluteUri));
            }
        }
    }
}