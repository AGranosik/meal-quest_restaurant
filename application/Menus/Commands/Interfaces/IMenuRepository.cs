using domain.Menus.ValueObjects.Identifiers;
using FluentResults;

namespace application.Menus.Commands.Interfaces
{
    public interface IMenuRepository
    {
        Task<Result<MenuId>> CreateMenuAsync(MenuId menu, CancellationToken cancellationToken);
    }
}
