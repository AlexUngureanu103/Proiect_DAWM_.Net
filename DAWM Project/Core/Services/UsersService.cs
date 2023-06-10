using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos.UserDtos;
using RestaurantAPI.Domain.Enums;
using RestaurantAPI.Domain.Mapping;
using RestaurantAPI.Domain.Models.Users;
using RestaurantAPI.Domain.ServicesAbstractions;
using RestaurantAPI.Exceptions;

namespace Core.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IAuthorizationService _authService;

        private readonly IDataLogger logger;

        public UsersService(IUnitOfWork unitOfWork, IAuthorizationService authService, IDataLogger logger)
        {
            this._unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
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

            if (string.IsNullOrEmpty(registerData.FirstName) || string.IsNullOrEmpty(registerData.LastName) || string.IsNullOrEmpty(registerData.Email)
                || string.IsNullOrEmpty(registerData.Password) || (int)registerData.Role > enumValues.Length - 1)
            {
                logger.LogWarn("Register data is not valid");
                return false;
            }

            var userByEmail = await _unitOfWork.UsersRepository.GetUserByEmail(registerData.Email);
            if (userByEmail != null)
            {
                logger.LogError($"E-mail: {registerData.Email} already exists in the database.");
                return false;
            }

            registerData.Password = _authService.HashPassword(registerData.Password);

            User user = UserMapping.MapToUser(registerData);
            logger.LogInfo($"User: {user.FirstName}  {user.LastName}, E-mail: {user.Email}, Role: {user.Role} has been registered successfully.");
            await _unitOfWork.UsersRepository.AddAsync(user);

            bool response = await _unitOfWork.SaveChangesAsync();

            return response;
        }

        public async Task<string> ValidateCredentials(LoginDto payload)
        {
            return await CredentialsValidator(payload, Role.Guest);
        }

        public async Task<string> ValidateAdminCredentials(LoginDto payload)
        {
            return await CredentialsValidator(payload, Role.Admin);
        }

        private async Task<string> CredentialsValidator(LoginDto payload, Role MinRole)
        {
            User userFromDb = await _unitOfWork.UsersRepository.GetUserByEmail(payload.Email);
            if (userFromDb == null)
            {
                logger.LogWarn($"Account with E-mail: {payload.Email} not found");
                return string.Empty;
            }

            bool passwordFine = _authService.VerifyHashedPassword(userFromDb.PasswordHash, payload.Password);
            if (!passwordFine)
            {
                logger.LogWarn("The inserted password is invalid");
                return string.Empty;
            }

            if (userFromDb.Role.CompareTo(MinRole) < 0)
            {
                logger.LogWarn($"User role: {userFromDb.Role} is unautorized. Min role required: {MinRole}");
                return string.Empty;
            }
            
            string role = userFromDb.Role.ToString();
            logger.LogInfo($"User with E-mail: {userFromDb.Email} logged in");
            return _authService.GetToken(userFromDb, role);
        }

        public async Task<bool> DeleteAccount(int id)
        {
            try
            {
                await _unitOfWork.UsersRepository.DeleteAsync(id);
            }
            catch (EntityNotFoundException exception)
            {
                logger.LogError(exception.Message, exception);

                return false;
            }

            bool response = await _unitOfWork.SaveChangesAsync();

            return response;
        }

        public async Task<bool> UpdateUserDetails(int userId, CreateOrUpdateUser payload)
        {
            User userFromDb = await _unitOfWork.UsersRepository.GetUserByEmail(payload.Email);
            if (userFromDb != null && userFromDb.Id != userId)
            {
                logger.LogWarn($"E-mail: {payload.Email} is already registered");
                return false;
            }

            payload.Password = _authService.HashPassword(payload.Password);

            User user = UserMapping.MapToUser(payload);

            try
            {
                await _unitOfWork.UsersRepository.UpdateAsync(userId, user);
            }
            catch (EntityNotFoundException exception)
            {
                logger.LogError(exception.Message, exception);

                return false;
            }
            catch (ArgumentNullException exception)
            {
                logger.LogError(exception.Message, exception);
                return false;

            }

            bool response = await _unitOfWork.SaveChangesAsync();

            return response;
        }

        public async Task<UserPublicData> GetUserPublicData(int userId)
        {
            User userFromDb = await _unitOfWork.UsersRepository.GetByIdAsync(userId);

            if (userFromDb == null)
            {
                string guest = "Guest";
                logger.LogWarn($"User with id: {userId} not found");

                return new UserPublicData { Email = guest, FirstName = guest, LastName = guest };
            }

            return UserMapping.MapToUserPublicData(userFromDb);
        }
    }
}
