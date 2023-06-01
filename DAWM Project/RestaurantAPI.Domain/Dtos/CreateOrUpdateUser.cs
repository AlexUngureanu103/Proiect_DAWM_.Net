﻿using RestaurantAPI.Domain.Enums;
using RestaurantAPI.Domain.Models.Users;

namespace RestaurantAPI.Domain.Dtos
{
    public class CreateOrUpdateUser
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }

        public PersonalDataDto PersonalDataDto { get; set; }
    }
}