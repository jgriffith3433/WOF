﻿using WOF.Application.Common.Mappings;
using WOF.Domain.Entities;

namespace WOF.Application.Ingredients.Queries.GetIngredientsWithPagination;

public class IngredientBriefDto : IMapFrom<Ingredient>
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? WalmartId { get; set; }
}
