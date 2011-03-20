using System;
using System.ComponentModel.Composition;
using URLExpander.Models;
using URLExpander.Serializers;
using URLExpander.ViewModels;

namespace URLExpander.UrlExpanders
{
    [Export(typeof(IUrlExpander))]
    public class PostlyUrlExpander : UrlExpanderBase
    {
        private static readonly Uri IsgdApiBaseUri = new Uri("http://posterous.com/api/");
        private static readonly XmlSerializer ExpandResponseDeserializer = new XmlSerializer();

        public override void IfCanExpand(Uri uri, Action<IExpandedUrlViewModel> callback)
        {
            if (uri.DnsSafeHost != "post.ly")
            {
                return;
            }

            DoCallback(uri, callback);
        }

        public void ExpandUrl(string shortUrl, Action<PostLyExpandResponse> callback)
        {
            var id = shortUrl.Substring(shortUrl.LastIndexOf('/') + 1);
            MakePostlyWebRequestAsync(
                ExpandResponseDeserializer,
                string.Format("getpost?id={0}", id),
                callback);
        }

        private void MakePostlyWebRequestAsync(ISerializer responseDeserializer, string relativeUrl, Action<PostLyExpandResponse> webRequestCallback)
        {
            MakeWebRequestAsync(
                responseDeserializer,
                new Uri(IsgdApiBaseUri, relativeUrl),
                webRequestCallback);
        }

        private void DoCallback(Uri uri, Action<IExpandedUrlViewModel> callback)
        {
            if (callback != null)
            {
                callback(new PostlyViewModel(this, uri.AbsoluteUri));
            }
        }
    }
}
