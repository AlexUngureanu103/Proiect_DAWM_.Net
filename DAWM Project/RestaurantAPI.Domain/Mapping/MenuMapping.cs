using RestaurantAPI.Domain.Dtos.MenuDtos;
using RestaurantAPI.Domain.Models.MenuRelated;

namespace RestaurantAPI.Domain.Mapping
{
    public static class MenuMapping
    {
        public static Menu MapToMenu(CreateOrUpdateMenu menu)
        {
            if (menu == null)
                return null;

            return new Menu
            {
                Name = menu.Name,
                Price = menu.Price,
                MenuItems = new(),
                ImageUrl = menu.ImageUrl
            };
        }

        public static MenuInfos MapToMenuInfos(Menu menu)
        {
            if (menu == null)
                return null;

            return new MenuInfos
            {
                MenuId = menu.Id,
                Name = menu.Name,
                Price = menu.Price,
                RecipesIds = menu.MenuItems
                .Select(i => i.RecipeId)
                .ToList(),
                ImageUrl = menu.ImageUrl
            };
        }

        public static MenuOrderInfo MapToMenuInfos(Menu menu, int quantity)
        {
            if (menu == null)
                return null;

            return new MenuOrderInfo
            {
                MenuId = menu.Id,
                Name = menu.Name,
                Price = menu.Price,
                RecipesIds = menu.MenuItems
                .Select(i => i.RecipeId)
                .ToList(),
                ImageUrl = menu.ImageUrl,
                Quantity = quantity
            };
        }

        public static MenuItemDto MapToMenuItemDto(MenuItem menuItem)
        {
            if (menuItem == null)
                return null;

            return new MenuItemDto
            {
                MenuId = menuItem.MenuId,
                RecipeId = menuItem.RecipeId
            };
        }
    }
}
