using DataLayer;
using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos;
using RestaurantAPI.Domain.Enums;
using RestaurantAPI.Domain.Mapping;
using RestaurantAPI.Domain.Models.Users;
using RestaurantAPI.Domain.ServicesAbstractions;

namespace Core.Services
{
    public class UsersService : IUsersService
    {
        private readonly UnitOfWork unitOfWork;

        private readonly IAuthorizationService _authService;

        private readonly IDataLogger logger;

        public UsersService(UnitOfWork unitOfWork, IAuthorizationService authService, IDataLogger logger)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this._authService = authService ?? throw new ArgumentNullException(nameof(authService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<bool> Register(CreateOrUpdateUser registerData)
        {
            if (registerData == null)
            {
                logger.LogWarn("Register data was null");
                return false;
            }

            Role[] enumValues = (Role[])Enum.GetValues(typeof(Role));

            if (registerData.FirstName.Trim() == string.Empty || registerData.LastName.Trim() == string.Empty || registerData.Email.Trim() == string.Empty
                || registerData.Password.Trim() == string.Empty || (int)registerData.Role > enumValues.Length - 1)
            {
                logger.LogWarn("Register data is not valid");
                return false;
            }

            var userByEmail = await unitOfWork.UsersRepository.GetUserByEmail(registerData.Email);
            if (userByEmail != null)
            {
                logger.LogError($"E-mail: {registerData.Email} already exists in the database.");
                return false;
            }

            registerData.Password = _authService.HashPassword(registerData.Password);

            User user = UserMapping.MapToUser(registerData);
            logger.LogInfo($"User: {user.PersonalData.FirstName}  {user.PersonalData.LastName}, E-mail: {user.Email}, Role: {user.Role} has been registered successfully.");
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
