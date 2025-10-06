using application.Menus.Commands.Interfaces;
using core.SimpleTypes;
using domain.Menus.Aggregates;
using domain.Menus.Aggregates.Entities;
using domain.Menus.ValueObjects;
using domain.Menus.ValueObjects.Identifiers;
using FluentResults;
using MassTransit.Util;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Database.MenuContext.Repositories;

// TODO: SHould not throw on the same menu 
internal class MenuRepository : IMenuRepository
{
    private readonly MenuDbContext _context;

    public MenuRepository(MenuDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task CreateRestaurantAsync(MenuRestaurant restaurant, CancellationToken cancellationToken)
    {
        _context.Restaurants.Add(restaurant);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result<MenuId>> CreateMenuAsync(Menu menu, CancellationToken cancellationToken)
    {
        await HandleCategoriesUniqueness(menu, cancellationToken);
        _context.Menus.Add(menu);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Ok(menu.Id!);
    }

    private async Task HandleCategoriesUniqueness(Menu menu, CancellationToken cancellationToken)
    {
        var allCategories = menu.Groups
            .SelectMany(g => g.Meals)
            .SelectMany(m => m.Categories)
            .ToList();

        var uniqueCategoryNames = allCategories
            .Select(c => c.Name)
            .DistinctBy(c => c.Value)
            .ToList();

        var dbCategories = await _context.Categories
            .Where(db => uniqueCategoryNames.Contains(db.Name))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var dbCategoryNames = new HashSet<string>(dbCategories.Select(c => c.Name.Value));

        var notExistingCategories = allCategories
            .Where(c => !dbCategoryNames.Contains(c.Name.Value))
            .GroupBy(c => c.Name.Value)
            .Select(g => g.First())
            .ToList();

        if (notExistingCategories.Count > 0)
        {
            _context.Categories.AddRange(notExistingCategories);
            await _context.SaveChangesAsync(cancellationToken);
            _context.Categories.
        }

        var dbCategoryLookup = dbCategories.ToDictionary(c => c.Name.Value, c => c.Id);

        foreach (var category in allCategories)
        {
            if (dbCategoryLookup.TryGetValue(category.Name.Value, out var id))
            {
                category.SetId(id!);
                _context.Categories.Attach(category);
            }
        }
    }
}