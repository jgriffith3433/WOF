using AutoMapper;
using WOF.Application.Common.Mappings;
using WOF.Domain.Entities;

namespace WOF.Application.CompletedOrders.Queries.GetCompletedOrders;

public class IngredientDto : IMapFrom<Ingredient>
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string WalmartId { get; set; }
}
