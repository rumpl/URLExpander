using URLExpander.Serializers;

namespace URLExpander
{
    using System;
    using System.ComponentModel.Composition;
    using System.Windows.Browser;

    using URLExpander.Models;
    using URLExpander.ViewModels;

    [Export(typeof(IUrlExpander))]
    public class IsgdUrlExpander : UrlExpanderBase
    {
        private static readonly Uri IsgdApiBaseUri = new Uri("http://is.gd/");
        private static readonly JsonSerializer ExpandResponseDeserializer = new JsonSerializer();

        public override void IfCanExpand(Uri uri, Action<IExpandedUrlViewModel> callback)
        {
            if (uri.DnsSafeHost != "is.gd")
            {
                return;
            }

            this.DoCallback(uri, callback);
        }

        public void ExpandUrl(string shortUrl, Action<IsgdExpandResponse> callback)
        {
            this.MakeIsgdWebRequestAsync(
                ExpandResponseDeserializer,
                string.Format("forward.php?shorturl={0}", HttpUtility.UrlEncode(shortUrl)),
                callback);
        }

        private void MakeIsgdWebRequestAsync(ISerializer responseDeserializer, string relativeUrl, Action<IsgdExpandResponse> webRequestCallback)
        {
            this.MakeWebRequestAsync(
                responseDeserializer,
                new Uri(IsgdApiBaseUri, relativeUrl + string.Format("&format=json")),
                webRequestCallback);
        }

        private void DoCallback(Uri uri, Action<IExpandedUrlViewModel> callback)
        {
            if (callback != null)
            {
                callback(new IsgdViewModel(this, uri.AbsoluteUri));
            }
        }
    }
}