using WOF.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<Recipe> Recipes { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
