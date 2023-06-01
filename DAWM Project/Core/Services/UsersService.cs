using DataLayer;
using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos;
using RestaurantAPI.Domain.Enums;
using RestaurantAPI.Domain.Mapping;
using RestaurantAPI.Domain.Models.Users;

namespace Core.Services
{
    public class UsersService
    {
        private readonly UnitOfWork unitOfWork;

        private AuthorizationService authService { get; set; }

        private readonly IDataLogger logger;

        public UsersService(UnitOfWork unitOfWork, AuthorizationService authService, IDataLogger logger)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.authService = authService ?? throw new ArgumentNullException(nameof(authService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

            bool response = await unitOfWork.SaveChangesAsync();

            return response;
        }

        public async Task<bool> DeleteAccount(int id)
        {
            await unitOfWork.UsersRepository.DeleteAsync(id);

            bool response;
            if (true)
            {
                response = await unitOfWork.SaveChangesAsync();
            }

            return response;
        }
    }
}
