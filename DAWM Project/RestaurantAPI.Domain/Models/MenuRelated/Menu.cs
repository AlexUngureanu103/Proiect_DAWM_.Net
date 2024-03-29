﻿namespace RestaurantAPI.Domain.Models.MenuRelated
{
    public class Menu : BaseEntity
    {
        public string Name { get; set; }

        public float Price { get; set; }

        public List<MenuItem> MenuItems { get; set; }

        public string ImageUrl { get; set; }
    }
}
