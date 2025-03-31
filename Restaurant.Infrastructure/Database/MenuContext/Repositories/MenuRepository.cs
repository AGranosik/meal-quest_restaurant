using application.Menus.Commands.Interfaces;
using domain.Menus.Aggregates;
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
        _context.Restaurants.Attach(menu.Restaurant);
        _context.Menus.Add(menu);
        await _context.SaveChangesAsync(cancellationToken);
        return menu.Id!;
    }

    private async Task HandleCategoriesUniqueness(Menu menu, CancellationToken cancellationToken)
    {
        var categoriesDomain = menu.Groups.SelectMany(g => g.Meals).SelectMany(m => m.Categories).ToList();
        var uniqueCategories = categoriesDomain.Select(c => c.Value).Distinct();

        var dbCategories = await _context.Categories.Where(db => uniqueCategories.Contains(db.Value))
            .Select(c => new
            {
                Name = c.Value,
                Id = EF.Property<int>(c, "CategoryID")
            })
            .ToListAsync(cancellationToken);
        var notExistingCategories = categoriesDomain.Where(c => dbCategories.All(db => db.Name != c.Value.Value)).Distinct().ToList();
        _context.Categories.AddRange(notExistingCategories);
        await _context.SaveChangesAsync(cancellationToken);
        foreach (var category in categoriesDomain)
        {
            var dbCategory = dbCategories.FirstOrDefault(db => db.Name == category.Value.Value);
            if (dbCategory == null)
                continue;
            
            _context.Categories.Entry(category).Property<int>("CategoryID").CurrentValue = dbCategory.Id;
            _context.Categories.Attach(category);
        }
        // var existingCategories = categoriesDomain.Where(c => !notExistingCategories.Contains(c)).ToList();
        // _context.Categories.AttachRange(existingCategories);
    }
}