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

        public Task<Result<MenuId>> CreateMenuAsync(Menu menu, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
