using application.Menus.Commands.Interfaces;
using domain.Menus.Aggregates;
using domain.Menus.ValueObjects.Identifiers;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Database.MenuContext.Repositories
{
    // TODO: SHould not throw on the same menu 
    internal class MenuRepository(MenuDbContext context) : IMenuRepository
    {
        private readonly MenuDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task CreateRestaurantAsync(RestaurantIdMenuId restaurant, CancellationToken cancellationToken)
        {
            var exists = await _context.Restaurants.AnyAsync(r => r.Value == restaurant.Value, cancellationToken);
            if (exists)
                return;

            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Result<MenuId>> CreateMenuAsync(Menu menu, CancellationToken cancellationToken)
        {
            _context.Menus.Add(menu);
            await _context.SaveChangesAsync(cancellationToken);
            return menu.Id!;
        }
    }
}
