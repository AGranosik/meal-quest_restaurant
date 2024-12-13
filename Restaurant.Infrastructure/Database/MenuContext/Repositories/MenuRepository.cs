using application.Menus.Commands.Interfaces;
using domain.Menus.Aggregates;
using domain.Menus.ValueObjects.Identifiers;
using FluentResults;

namespace infrastructure.Database.MenuContext.Repositories
{
    // TODO: SHould not throw on the same menu 
    public class MenuRepository(MenuDbContext context) : IMenuRepository
    {
        private readonly MenuDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task AddRestaurantAsync(RestaurantIdMenuId restaurant, CancellationToken cancellationToken)
        {
            _context.Restaurants.Add(new RestaurantIdMenuId(restaurant.Value));
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
