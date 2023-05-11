using WOF.Application.Common.Interfaces;
using WOF.Application.Walmart.Requests;
using WOF.Infrastructure.Walmart.Responses;

namespace WOF.Infrastructure.Services;

public class WalmartApiService : IWalmartApiService
{
    public ISearchResponse Search(string query)
    {
        var searchRequest = new SearchRequest
        {
            query = query
        };

        return searchRequest.GetResponse<SearchResponse>().Result;
    }

    public IItemResponse GetItem(string id)
    {
        var itemRequest = new ItemRequest
        {
            id = id
        };

        return itemRequest.GetResponse<ItemResponse>().Result;
    }

    public IItemResponse GetItem(long? id)
    {
        return GetItem(id.ToString());
    }
}
