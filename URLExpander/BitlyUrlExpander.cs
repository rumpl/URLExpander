﻿namespace URLExpander
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization.Json;
    using System.Windows.Browser;

    using URLExpander.Models;
    using URLExpander.ViewModels;

    public class BitlyUrlExpander : UrlExpanderBase
    {
        private const string BitlyUsername = "IS THERE A WAY THAT I CAN STORE THIS IN A CONFIG FILE OR SOMETHING...";

        private const string BitlyApiKey = "HOW CAN I KEEP FROM PUTTING THIS INTO SOURCE CONTROL...?";

        private static readonly Uri BitlyApiBaseUri = new Uri("http://api.bitly.com/v3/");

        private static readonly DataContractJsonSerializer DomainResponseDeserializer = new DataContractJsonSerializer(typeof(BitlyDomainResponse));
        private static readonly DataContractJsonSerializer ExpandResponseDeserializer = new DataContractJsonSerializer(typeof(BitlyExpandResponse));

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

        private readonly Action<BitlyViewModel> callback;

        public BitlyUrlExpander(Action<BitlyViewModel> callback)
        {
            this.callback = callback;
        }

        public void TryExpandAsync(Uri uri)
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
                    this.DoCallback(uri);
                }
            }
            else
            {
                this.MakeBitlyWebRequestAsync(
                    string.Format("bitly_pro_domain?domain={0}", HttpUtility.UrlEncode(uri.DnsSafeHost)),
                    (BitlyDomainResponse result) =>
                        {
                            Domains[uri.DnsSafeHost] = result.Data.IsBitlyProDomain;

                            if (!result.Data.IsBitlyProDomain)
                            {
                                return;
                            }

                            this.DoCallback(uri);
                        });
            }
        }

        private void MakeBitlyWebRequestAsync<T>(string relativeUrl, Action<T> webRequestCallback) where T : class, IBitlyResponse
        {
            this.MakeWebRequestAsync(
                DomainResponseDeserializer,
                new Uri(
                    BitlyApiBaseUri,
                    relativeUrl +
                    string.Format("&apiKey={0}&login={1}&format=json", HttpUtility.UrlEncode(BitlyApiKey), HttpUtility.UrlEncode(BitlyUsername))),
                webRequestCallback);
        }

        private void DoCallback(Uri uri)
        {
            if (this.callback != null)
            {
                this.callback(new BitlyViewModel(uri.AbsoluteUri));
            }
        }
    }
}