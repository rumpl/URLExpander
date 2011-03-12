namespace URLExpander
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Seesmic.Sdp.Extensibility;

    [Export(typeof(ITimelineItemProcessor))]
    public class UrlExpanderProcessor : ITimelineItemProcessor
    {
        /// <summary>
        /// A regular expression to match a URL in free text (also includes wrapping parenthesis, to be removed manually after matching)
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

        [ImportMany(typeof(IUrlExpander))]
        internal IEnumerable<IUrlExpander> _urlExpanders;

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
            foreach (var bitlyExpander in this._urlExpanders)
            {
                foreach (var uri in GetShortUrls(timelineItemContainer))
                {
                    bitlyExpander.IfCanExpand(
                        uri,
                        viewModel =>
                        {
                            var attachment = new UrlExpanderAttachment(viewModel);
                            timelineItemContainer.AddAttachment(attachment);
                        });
                }
            }
        }

        private static IEnumerable<Uri> GetShortUrls(TimelineItemContainer timelineItemContainer)
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
                urls = ShortUrlRegex.Matches(timelineItemContainer.TimelineItem.Text).Cast<Match>().Select(m =>
                    {
                        if (m.Value.StartsWith("(", StringComparison.Ordinal) && m.Value.EndsWith(")", StringComparison.Ordinal))
                        {
                            return m.Value.Substring(1, m.Value.Length - 2);
                        }

                        return m.Value;
                    });
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
}