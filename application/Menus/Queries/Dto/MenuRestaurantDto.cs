using domain.Menus.ValueObjects;

namespace application.Menus.Queries.Dto;

public sealed record MenuRestaurantDto(string Name, List<MenuGroupDto> Groups);

public sealed record MenuGroupDto(string Name, List<MealDto> Meals);

public sealed record MealDto(string Name, List<string> CategoriesName, decimal Price, List<IngredientDto> Ingredients);

public sealed record IngredientDto(string Name);