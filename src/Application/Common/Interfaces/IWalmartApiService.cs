﻿
using WOF.Application.Common.Models.Walmart.Responses;

namespace WOF.Application.Common.Interfaces;

public interface IWalmartApiService
{
    SearchResponse Search(string query);
    ItemResponse GetItem(string id);
    ItemResponse GetItem(long? id);
}

//public interface ISearchResponse
//{
//    public string query { get; set; }
//    public string sort { get; set; }
//    public string responseGroup { get; set; }
//    public long totalResults { get; set; }
//    public long start { get; set; }
//    public long numItems { get; set; }
//    public List<ItemResponse> items { get; set; }
//}

//public interface IItemResponse
//{
//    public long itemId { get; set; }
//    public long parentItemId { get; set; }
//    public string name { get; set; }
//    public float msrp { get; set; }
//    public float salePrice { get; set; }
//    public string upc { get; set; }
//    public string categoryPath { get; set; }
//    public string brandName { get; set; }
//    public string thumbnailImage { get; set; }
//    public string mediumImage { get; set; }
//    public string largeImage { get; set; }
//    public string productTrackingUrl { get; set; }
//    public bool ninetySevenCentShipping { get; set; }
//    public float standardShipRate { get; set; }
//    public float twoThreeDayShippingRate { get; set; }
//    public float overnightShippingRate { get; set; }
//    public bool marketplace { get; set; }
//    public bool shipToStore { get; set; }
//    public bool freeShipToStore { get; set; }
//    public string modelNumber { get; set; }
//    public string productUrl { get; set; }
//    public string customerRating { get; set; }
//    public long numReviews { get; set; }
//    public string customerRatingImage { get; set; }
//    public string categoryNode { get; set; }
//    public bool bundle { get; set; }
//    public bool clearance { get; set; }
//    public bool preOrder { get; set; }
//    public string productStock { get; set; }
//    public bool availableOnline { get; set; }
//    public string shortDescription { get; set; }
//    public string longDescription { get; set; }
//    public bool specialBuy { get; set; }
//    public string sellerInfo { get; set; }
//    public string color { get; set; }
//    public bool rollBack { get; set; }
//    public bool freight { get; set; }
//    public string size { get; set; }
//}