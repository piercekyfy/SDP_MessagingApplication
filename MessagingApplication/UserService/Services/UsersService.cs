using Shared.Exceptions;
using UserService.DTOs;
using UserService.Exceptions;
using UserService.Models;
using UserService.Repository;

namespace UserService.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUserRepository userRepository;
        private readonly IUserExchangeService userExchangeService;

        public UsersService(IUserRepository userRepository, IUserExchangeService userExchangeService) 
        { 
            this.userRepository = userRepository;
            this.userExchangeService = userExchangeService;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await userRepository.GetAllAsync();
        }

        public async Task<User?> GetByUniqueNameAsync(string uniqueName)
        {
            return await userRepository.GetByUniqueNameAsync(uniqueName);
        }

        public async Task CreateAsync(User user)
        {
            user.DateCreated = DateTime.UtcNow;
            await userRepository.CreateAsync(user);
            await userExchangeService.PublishUserCreatedAsync(user);
        }

        public async Task<User> UpdateAsync(string uniqueName, UpdateUserRequest request)
        {
            User? user = await userRepository.GetByUniqueNameAsync(uniqueName);
            if (user == null)
                throw new UserNotFoundException(uniqueName);

            request.Apply(user);

            if (user.DisplayName.Trim().Length <= 0)
                throw new DomainException("DisplayName Length <= 0.") { DisplayMessage = "User.DisplayName length must be greater than or equal to 1."};

            if (user.Email.Trim().Length <= 3)
                throw new DomainException("Email Length <= 3.") { DisplayMessage = "User.Email length must be greater than or equal to 3." };

            await userRepository.UpdateAsync(user);
            await userExchangeService.PublishUserUpdatedAsync(user);
            return user;
        }
    }
}
