using WOF.Application.Common.Interfaces;
using WOF.Application.Common.Models.Walmart.Requests;
using WOF.Application.Common.Models.Walmart.Responses;

namespace WOF.Application.Common.Models.Walmart;

public class WalmartApiService : IWalmartApiService
{
    public SearchResponse Search(string query)
    {
        var searchRequest = new SearchRequest
        {
            query = query
        };

        return searchRequest.GetResponse<SearchResponse>().Result;
    }

    public ItemResponse GetItem(string id)
    {
        var itemRequest = new ItemRequest
        {
            id = id
        };

        return itemRequest.GetResponse<ItemResponse>().Result;
    }

    public ItemResponse GetItem(long? id)
    {
        return GetItem(id.ToString());
    }
}
