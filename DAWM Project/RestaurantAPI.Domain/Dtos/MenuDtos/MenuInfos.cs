﻿namespace RestaurantAPI.Domain.Dtos.MenuDtos
{
    public class MenuInfos
    {
        public int MenuId { get; set; }

        public string Name { get; set; }

        public float Price { get; set; }

        public List<int> RecipesIds { get; set; }

        public string ImageUrl { get; set; }
    }
}
