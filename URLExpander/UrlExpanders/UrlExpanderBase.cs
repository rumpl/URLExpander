using System.ComponentModel.Composition;
using Seesmic.Sdp.Extensibility;
using URLExpander.Serializers;

namespace URLExpander
{
    using System;
    using System.Net;

    using URLExpander.Models;
    using URLExpander.ViewModels;

    public abstract class UrlExpanderBase: IUrlExpander
    {
        protected void MakeWebRequestAsync<T>(ISerializer deserializer, Uri address, Action<T> callback) where T : class, IResponse
        {
            var webClient = new WebClient();
            webClient.OpenReadCompleted += (sender, args) =>
                {
                    if (args == null || args.Error != null) return;

                    var result = deserializer.ReadObject<T>(args.Result);
                    if (result == null || !result.IsSuccessfulResponse)
                    {
                        return;
                    }

                    if (callback != null)
                    {
                        callback(result);
                    }
                };

            webClient.OpenReadAsync(address);
        }

        public abstract void IfCanExpand(Uri uri, Action<IExpandedUrlViewModel> callback);
    }
}