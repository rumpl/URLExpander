using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using Seesmic.Sdp.Extensibility;

namespace URLExpander
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Runtime.Serialization.Json;

    using URLExpander.Models;
    using URLExpander.ViewModels;

    [Export(typeof(ITimelineItemProcessor))]
    public class URLExpanderProcessor : ITimelineItemProcessor
    {
        /// <summary>
        /// A regular expression to match a URL in free text (also includes wrapping paranthesis, to be removed manually after matching)
        /// </summary>
        /// <remarks>
        /// from http://www.codinghorror.com/blog/2008/10/the-problem-with-urls.html
        ///  \(?\bhttp://
        ///      Literal (, zero or one repetitions
        ///      First or last character in a word
        ///      http://
        ///  Any character in this class: [-A-Za-z0-9+&@#/%?=~_()|!:,.;], any number of repetitions
        ///  Any character in this class: [-A-Za-z0-9+&@#/%=~_()|]
        /// </remarks>
        private static readonly Regex ShortUrlRegex = new Regex(@"\(?\bhttp://[-a-z0-9+&@#/%?=~_()|!:,.;]*[-a-z0-9+&@#/%=~_()|]", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        public bool Filter(TimelineItemContainer timelineItemContainer)
        {
            return true;
        }

        public void Deliver(TimelineItemContainer timelineItemContainer)
        {
        }

        public void Remove(TimelineItemContainer timelineItemContainer)
        {
        }

        public void Process(TimelineItemContainer timelineItemContainer)
        {
            var uris = this.GetShortUrls(timelineItemContainer);

            var regex = new Regex("http://(bit.ly|4sq.com|tcrn.ch|nyti.ms|amzn.to|binged.it|huff.to|pep.si|n.pr|j.mp)/(?<Id>[0-9a-zA-Z]*)");
            if (!regex.IsMatch(timelineItemContainer.TimelineItem.Text)) return;
            var match = regex.Match(timelineItemContainer.TimelineItem.Text);
            var attachement = new URLExpanderAttachement(match.Value);
            timelineItemContainer.AddAttachment(attachement);
        }

        private IEnumerable<Uri> GetShortUrls(TimelineItemContainer timelineItemContainer)
        {
            IEnumerable<string> urls = null;
            if (timelineItemContainer.Metadata.Any(md => md.Key == "com.seesmic.timelineItem.UrlsDictionary"))
            {
                var urlsDictionaryMetadata = timelineItemContainer.Metadata.First(md => md.Key == "com.seesmic.timelineItem.UrlsDictionary");
                var urlsDictionary = urlsDictionaryMetadata.Value as IDictionary<string, string>;
                if (urlsDictionary != null)
                {
                    urls = urlsDictionary.Keys;
                }
            }

            if (urls == null)
            {
                urls = ShortUrlRegex.Matches(timelineItemContainer.TimelineItem.Text).Cast<Match>().Select(m => m.Value);
            }

            // convert to URI, only return short URLs (no querystring, only slash is separating host and path)
            return urls.Select(urlText =>
                        {
                            Uri url;
                            if (Uri.TryCreate(urlText, UriKind.Absolute, out url))
                            {
                                return url;
                            }

                            return null;
                        })
                       .Where(url => url != null && string.IsNullOrEmpty(url.Query) && url.LocalPath.LastIndexOf('/') < 1);
        }
    }

    public class BitlyUrlExpander
    {
        private static readonly DataContractJsonSerializer Deserlializer = new DataContractJsonSerializer(typeof(BitlyResponse));
        private static readonly HashSet<string> Domains = new HashSet<string>(StringComparer.Ordinal) {
                                                                                                          "bit.ly",
                                                                                                          "j.mp"
                                                                                                      };

        private Action<BitlyViewModel> callback;

        public BitlyUrlExpander(Action<BitlyViewModel> callback)
        {
            this.callback = callback;
        }

        public void TryExpandAsync(Uri uri)
        {
            if (Domains.Contains(uri.DnsSafeHost))
            {
                DoCallback(uri);
            }
            else
            {
                var webClient = new WebClient();
                webClient.OpenReadCompleted += (sender, args) =>
                    {
                        if (args.Error != null) return;

                        var result = Deserlializer.ReadObject(args.Result) as BitlyResponse;
                        if (result == null) return;

                        if (result.Data.IsBitlyProDomain)
                        {
                            Domains.Add(uri.DnsSafeHost);
                            this.DoCallback(uri);
                        }
                    };
                webClient.OpenReadAsync(new Uri(string.Format("http://api.bitly.com/v3/bitly_pro_domain?domain={0}&apiKey={1}&login={2}&format=json", uri.DnsSafeHost)));
            }
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