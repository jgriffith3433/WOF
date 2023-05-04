namespace WOF.Application.Walmart.Responses;

public class SearchResponse
{
    public string query { get; set; }
    public string sort { get; set; }
    public string responseGroup { get; set; }
    public long totalResults { get; set; }
    public long start { get; set; }
    public long numItems { get; set; }
    public List<ItemResponse> items { get; set; }
}