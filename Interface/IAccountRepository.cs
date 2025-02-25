using acadgest.Dto.AppUser;
using acadgest.Models.Results;
using acadgest.Models.User;

namespace acadgest.Interface
{
    public interface IAccountRepository
    {
        public Task<List<AppUserDto>> GetAllAsync();
        public Task<AppUser?> GetByIdAsync(Guid id);
        public Task<List<string>> GetRolesAsync();
        public Task<RegisterResults> RegisterAsync(RegisterViewModel registerViewModel);
        public Task<LoginResults> LoginAsync(LoginViewModel loginViewModel);
        public Task<EditUserResults> EditAsync(EditUserViewModel editUser);
        public Task<bool> LogoutAsync();
        public Task<DeleteResults> DeletAsync(Guid userId);

    }
}