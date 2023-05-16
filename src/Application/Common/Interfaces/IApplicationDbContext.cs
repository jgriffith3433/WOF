using WOF.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace WOF.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    ChangeTracker ChangeTracker { get; }

    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<Recipe> Recipes { get; }

    DbSet<CookedRecipe> CookedRecipes { get; }

    DbSet<ConsumedCookedRecipe> ConsumedCookedRecipes { get; }

    DbSet<CookedRecipeCalledIngredient> CookedRecipeCalledIngredients { get; }

    DbSet<Product> Products { get; }

    DbSet<ProductStock> ProductStocks { get; }

    DbSet<CalledIngredient> CalledIngredients { get; }

    DbSet<CompletedOrder> CompletedOrders { get; }

    DbSet<CompletedOrderProduct> CompletedOrderProducts { get; }

    DbSet<ChatCommand> ChatCommands { get; }

    DbSet<ChatConversation> ChatConversations { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
