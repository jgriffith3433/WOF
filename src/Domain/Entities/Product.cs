﻿namespace WOF.Domain.Entities;

public class Product : BaseAuditableEntity
{
    public string Name { get; set; }
    public long? WalmartId { get; set; }
    public string? WalmartLink { get; set; }
    public string? WalmartSize { get; set; }
    public string? WalmartItemResponse { get; set; }
    public string? WalmartSearchResponse { get; set; }
    public string? Error { get; set; }
    public float Size { get; set; }
    public float Price { get; set; }
    public bool Verified { get; set; }
    public UnitType UnitType { get; set; }
    public IList<CompletedOrderProduct> CompletedOrderProducts { get; private set; } = new List<CompletedOrderProduct>();
    public ProductStock ProductStock { get; set; }
}
