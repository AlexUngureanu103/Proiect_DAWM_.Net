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
                RecipiesIds = menu.MenuItems?.Select(i => i.RecipeId).ToList()
            };
        }
    }
}
