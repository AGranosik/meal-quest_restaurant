using application.Menus.Queries.Dto;
using application.Menus.Queries.Interfaces;
using domain.Menus.ValueObjects.Identifiers;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Database.MenuContext.Repositories;

internal sealed class ReadMenuRepository : IReadMenuRepository
{
    private readonly MenuDbContext _dbContext;

    public ReadMenuRepository(MenuDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<MenuRestaurantDto?> GetRestaurantMenu(int restaurantId, CancellationToken cancellationToken)
        => _dbContext.Menus
            .Where(m => m.Restaurant.Id! == new RestaurantIdMenuId(restaurantId))
            .Select(m =>
                new MenuRestaurantDto(m.Name.Value.Value,
                m.Groups.Select(g => new MenuGroupDto(
                    g.GroupName.Value.Value, 
                        g.Meals.Select(m => new MealDto(
                            m.Name!.Value.Value!,
                                m.Categories.Select(c => c.Name.Value).ToList(),
                            m.Price!.Value,
                            m.Ingredients!.Select(i => new IngredientDto(
                                i.Name.Value
                                )).ToList()
                            )).ToList()
                    )).ToList()
                )
            ).FirstOrDefaultAsync(cancellationToken);
}