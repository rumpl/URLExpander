namespace URLExpander
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Windows.Browser;

    public class BitlyService
    {
        private static readonly HashSet<string> domains = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
                                                                                                                    "bit.ly",
                                                                                                                    "j.mp",
                                                                                                                    "4sq.com",
                                                                                                                    "tcrn.ch",
                                                                                                                    "nyti.ms",
                                                                                                                    "amzn.to",
                                                                                                                    "binged.it",
                                                                                                                    "huff.to",
                                                                                                                    "pep.si",
                                                                                                                    "n.pr"
                                                                                                                };

        public void bool CanExpandUrl(Uri url)
        {
            if (domains.Contains(url.DnsSafeHost))
            {
                return true;
            }

            var client = new WebClient();
            var uriString = string.Format("http://api.bitly.com/v3/bitly_pro_domain?domain={0}&apiKey={1}&login={2}&format=json", HttpUtility.UrlEncode(url.DnsSafeHost));
            client.OpenReadCompleted += (x, y) => y.Result.Close();

            client.OpenReadAsync(new Uri(uriString));

        }
    }
}
