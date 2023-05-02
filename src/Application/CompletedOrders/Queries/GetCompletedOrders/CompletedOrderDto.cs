using WOF.Application.Common.Mappings;
using WOF.Domain.Entities;

namespace WOF.Application.CompletedOrders.Queries.GetCompletedOrders;

public class CompletedOrderDto : IMapFrom<CompletedOrder>
{
    public CompletedOrderDto()
    {
        Ingredients = new List<IngredientDto>();
    }

    public int Id { get; set; }

    public string? UserImport { get; set; }

    public IList<IngredientDto> Ingredients { get; set; }
}
