using application.Menus.Commands.Interfaces;
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
        var categoriesDomain = menu.Groups.SelectMany(g => g.Meals).SelectMany(m => m.Categories).ToList();
        var uniqueCategories = categoriesDomain.Select(c => c.Name).Distinct();

        var dbCategories = await _context.Categories
            .Where(db => uniqueCategories.Contains(db.Name))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        //TODO: AVOID LOH ALLOCATIONS
        var notExistingCategories = categoriesDomain.Where(c => dbCategories.All(db => db.Name.Value != c.Name.Value)).Distinct().ToList();
        _context.Categories.AddRange(notExistingCategories);
        await _context.SaveChangesAsync(cancellationToken);
        
        foreach (var dbCategory in dbCategories)
        {
            var category = categoriesDomain.First(cd => cd.Name.Value == dbCategory.Name.Value);
            category.SetId(dbCategory.Id!);
            _context.Categories.Attach(category);
        }
    }
}