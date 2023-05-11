using System.Collections.Specialized;
using Newtonsoft.Json;
using WOF.Application.Common.Interfaces;
using WOF.Application.Common.Security;

namespace WOF.Application.Walmart.Requests;

public class ItemRequest : IWalmartApiRequest
{
    public ItemRequest()
    {
    }
    public string id { get; set; }

    public async Task<T> GetResponse<T>()
    {
        using (var client = new WalmartWebClient())
        {
            var t1 = client.GetTimestampInJavaMillis();
            var requiredHeaders = new NameValueCollection
                {
                    { "WM_CONSUMER.ID", "b8159879-e6cc-459a-888d-a47da6d224a3" },
                    { "WM_CONSUMER.INTIMESTAMP", t1 },
                    { "WM_SEC.KEY_VERSION", "3" },
                };
            client.Headers.Add(requiredHeaders);
            client.Headers.Add("WM_SEC.AUTH_SIGNATURE", client.GetWalmartSignature(client.Key, requiredHeaders[0], requiredHeaders[1], requiredHeaders[2]));

            client.BaseAddress = "https://developer.api.walmart.com";
            var jsonResponse = await client.DownloadStringTaskAsync(string.Format("/api-proxy/service/affil/product/v2/items/{0}", id));
            return JsonConvert.DeserializeObject<T>(jsonResponse);
        }
    }
}