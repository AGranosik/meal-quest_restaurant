using application.Menus.Commands.Interfaces;
using domain.Menus.Aggregates.Entities;
using domain.Menus.ValueObjects.Identifiers;
using FluentResults;
using Npgsql.Internal;

namespace infrastructure.Database.MenuContext.Repositories
{
    public class MenuRepository(MenuDbContext context) : IMenuRepository
    {
        private readonly MenuDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<Result<MenuId>> CreateMenuAsync(Menu menu, CancellationToken cancellationToken)
        {
            _context.Menus.Add(menu);
            await _context.SaveChangesAsync(cancellationToken);
            return menu.Id!;
        }
    }
}
