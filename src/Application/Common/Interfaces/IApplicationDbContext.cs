using WOF.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<Recipe> Recipes { get; }

    DbSet<Product> Products { get; }

    DbSet<ProductStock> ProductStocks { get; }

    DbSet<CalledIngredient> CalledIngredients { get; }

    DbSet<CompletedOrder> CompletedOrders { get; }

    DbSet<CompletedOrderProduct> CompletedOrderProducts { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
