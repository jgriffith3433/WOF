﻿namespace WOF.Domain.Entities;

public class CalledIngredient : BaseAuditableEntity
{
    public string Name { get; set; }

    public int ProductId { get; set; }

    public ProductStock ProductStock { get; set; } = null!;

    public IList<Recipe> Recipes { get; private set; } = new List<Recipe>();
}
