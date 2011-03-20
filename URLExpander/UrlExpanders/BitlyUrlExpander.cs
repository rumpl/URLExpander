using URLExpander.Serializers;

namespace URLExpander
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Windows.Browser;

    using Models;
    using ViewModels;

    [Export(typeof(IUrlExpander))]
    public class BitlyUrlExpander : UrlExpanderBase
    {
        private const string BitlyUsername = "IS THERE A WAY THAT I CAN STORE THIS IN A CONFIG FILE OR SOMETHING...";

        private const string BitlyApiKey = "HOW CAN I KEEP FROM PUTTING THIS INTO SOURCE CONTROL...?";

        private static readonly Uri BitlyApiBaseUri = new Uri("http://api.bitly.com/v3/");

        private static readonly JsonSerializer DomainResponseDeserializer = new JsonSerializer();
        private static readonly JsonSerializer ExpandResponseDeserializer = new JsonSerializer();

        private static readonly Dictionary<string, bool> Domains = new Dictionary<string, bool>(100, StringComparer.Ordinal) {
                                                                                                                                 { "bit.ly", true },
                                                                                                                                 { "j.mp", true },
                                                                                                                                 { "4sq.com", true },
                                                                                                                                 { "tcrn.ch", true },
                                                                                                                                 { "nyti.ms", true },
                                                                                                                                 { "amzn.to", true },
                                                                                                                                 { "binged.it", true },
                                                                                                                                 { "huff.to", true },
                                                                                                                                 { "pep.si", true },
                                                                                                                                 { "n.pr", true }
                                                                                                                             };

        public override void IfCanExpand(Uri uri, Action<IExpandedUrlViewModel> callback)
        {
            // per http://code.google.com/p/bitly-api/wiki/ApiDocumentation#/v3/bitly_pro_domain
            // "bitly.pro domains are restricted to less than 15 characters in length"
            if (uri.DnsSafeHost.Length > 14)
            {
                return;
            }

            bool isBitlyDomain;
            if (Domains.TryGetValue(uri.DnsSafeHost, out isBitlyDomain))
            {
                if (isBitlyDomain)
                {
                    DoCallback(uri, callback);
                }
            }
            else
            {
                MakeBitlyWebRequestAsync(
                    DomainResponseDeserializer, 
                    string.Format("bitly_pro_domain?domain={0}", HttpUtility.UrlEncode(uri.DnsSafeHost)), 
                    (BitlyDomainResponse result) =>
                    {
                        Domains[uri.DnsSafeHost] = result.Data.IsBitlyProDomain;

                        if (!result.Data.IsBitlyProDomain)
                        {
                            return;
                        }

                        DoCallback(uri, callback);
                    });
            }
        }

        public void ExpandUrl(string shortUrl, Action<BitlyExpandResponse> callback)
        {
            MakeBitlyWebRequestAsync(
                ExpandResponseDeserializer,
                string.Format("expand?shortUrl={0}", HttpUtility.UrlEncode(shortUrl)),
                callback);
        }

        public void GetNumberOfClicks(string shortUrl, Action<BitlyExpandResponse> callback)
        {
            MakeBitlyWebRequestAsync(
                ExpandResponseDeserializer,
                string.Format("clicks?shortUrl={0}", HttpUtility.UrlEncode(shortUrl)),
                callback);
        }

        private void MakeBitlyWebRequestAsync<T>(ISerializer responseDeserializer, string relativeUrl, Action<T> webRequestCallback) where T : class, IResponse
        {
            MakeWebRequestAsync(
                responseDeserializer,
                new Uri(
                    BitlyApiBaseUri,
                    relativeUrl + string.Format("&apiKey={0}&login={1}&format=json", HttpUtility.UrlEncode(BitlyApiKey), HttpUtility.UrlEncode(BitlyUsername))),
                webRequestCallback);
        }

        private void DoCallback(Uri uri, Action<IExpandedUrlViewModel> callback)
        {
            if (callback != null)
            {
                callback(new BitlyViewModel(this, uri.AbsoluteUri));
            }
        }
    }
}