namespace URLExpander
{
    using System;
    using System.Net;
    using System.Runtime.Serialization.Json;

    using URLExpander.Models;

    public abstract class UrlExpanderBase
    {
        protected void MakeWebRequestAsync<T>(DataContractJsonSerializer deserializer, Uri address, Action<T> callback) where T : class, IBitlyResponse
        {
            var webClient = new WebClient();
            webClient.OpenReadCompleted += (sender, args) =>
                {
                    if (args.Error != null) return;

                    var result = deserializer.ReadObject(args.Result) as T;
                    if (result == null || result.StatusCode != 200) return;

                    if (callback != null)
                    {
                        callback(result);
                    }
                };

            webClient.OpenReadAsync(address);
        }
    }
}