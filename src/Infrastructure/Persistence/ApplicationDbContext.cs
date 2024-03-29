﻿using System.Reflection;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using WOF.Infrastructure.Identity;
using WOF.Infrastructure.Persistence.Interceptors;
using Duende.IdentityServer.EntityFramework.Options;
using MediatR;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace WOF.Infrastructure.Persistence;

public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IOptions<OperationalStoreOptions> operationalStoreOptions,
        IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) 
        : base(options, operationalStoreOptions)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    public override ChangeTracker ChangeTracker => base.ChangeTracker;

    public DbSet<TodoList> TodoLists => Set<TodoList>();

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public DbSet<Recipe> Recipes => Set<Recipe>();

    public DbSet<CookedRecipe> CookedRecipes => Set<CookedRecipe>();

    public DbSet<ConsumedCookedRecipe> ConsumedCookedRecipes => Set<ConsumedCookedRecipe>();

    public DbSet<CookedRecipeCalledIngredient> CookedRecipeCalledIngredients => Set<CookedRecipeCalledIngredient>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<ProductStock> ProductStocks => Set<ProductStock>();

    public DbSet<CalledIngredient> CalledIngredients => Set<CalledIngredient>();

    public DbSet<CompletedOrder> CompletedOrders => Set<CompletedOrder>();

    public DbSet<CompletedOrderProduct> CompletedOrderProducts => Set<CompletedOrderProduct>();

    public DbSet<ChatCommand> ChatCommands => Set<ChatCommand>();

    public DbSet<ChatConversation> ChatConversations => Set<ChatConversation>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
}
