using DataLayer;
using RestaurantAPI.Domain.Dtos;
using RestaurantAPI.Domain.Enums;
using RestaurantAPI.Domain.Mapping;
using RestaurantAPI.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class UsersService
    {
        private readonly UnitOfWork unitOfWork;

        private AuthorizationService authService { get; set; }
        public UsersService(UnitOfWork unitOfWork, AuthorizationService authService)
        {
            this.unitOfWork = unitOfWork;
            this.authService = authService;
        }
        public async Task<bool> Register(CreateOrUpdateUser registerData)
        {
            if (registerData == null)

            {
                return false;
            }

            Role[] enumValues = (Role[])Enum.GetValues(typeof(Role));

            if (registerData.FirstName.Trim() == string.Empty || registerData.LastName.Trim() == string.Empty || registerData.Email.Trim() == string.Empty
                || registerData.Password.Trim() == string.Empty || (int)registerData.Role > enumValues.Length - 1)
            {
                return false;
            }

            registerData.Password = authService.HashPassword(registerData.Password);

            User user = UserMapping.MapToUser(registerData);           

            await unitOfWork.UsersRepository.AddAsync(user);

            bool response = await unitOfWork.SaveChanges();

            return response;
        }

        public async Task<bool> DeleteAccount(int id)
        {
            bool response = await unitOfWork.UsersRepository.DeleteAsync(id) is not null;

            if (response)
            {
                response = await unitOfWork.SaveChanges();
            }

            return response;
        }
    }
}
