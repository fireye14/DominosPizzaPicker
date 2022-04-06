using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DominosPizzaPicker.Client.Models.Managers
{
    public class CustomDelegatingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // If it's one of our Get requests that need extending. 
            if (request.Method == HttpMethod.Get)
            {
                if (request.RequestUri.PathAndQuery.Contains("/tables/Pizza"))
                {
                    var uriBuilder = new UriBuilder(request.RequestUri);
                    var uriBuilderQuery = uriBuilder.Query;
                    if (!uriBuilderQuery.Contains("$expand"))
                    {
                        if (string.IsNullOrEmpty(uriBuilderQuery))
                        {
                            uriBuilderQuery = string.Empty;
                        }
                        else
                        {
                            uriBuilderQuery = uriBuilderQuery + "&";
                        }

                        uriBuilderQuery = uriBuilderQuery + "$expand=Topping1";
                        uriBuilder.Query = uriBuilderQuery.TrimStart('?');
                        request.RequestUri = uriBuilder.Uri;
                    }
                }
            }

            var result = await base.SendAsync(request, cancellationToken);
            return result;
        }

    }
}