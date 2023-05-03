using WOF.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<Recipe> Recipes { get; }

    DbSet<Ingredient> Ingredients { get; }

    DbSet<Stock> Stocks { get; }

    DbSet<CalledIngredient> CalledIngredients { get; }

    DbSet<CompletedOrder> CompletedOrders { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
