using System.Security.Claims;
using acadgest.Dto.AppUser;
using acadgest.Interface;
using acadgest.Mappers;
using acadgest.Models.Results;
using acadgest.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace acadgest.Data.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountRepository(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        // ---------------------------------------------DELETE---------------------------------------------------
        public async Task<DeleteResults> DeletAsync(Guid userId)
        {
            var deleteResults = new DeleteResults
            {
                DeleteSucceded = false
            };

            // Pega o usuário pelo ID
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            // Verifica se o usuário existe
            if (user == null)
            {
                deleteResults.Error = "Usuário não encontrado.";
                return deleteResults;
            }

            // Pega as roles do usuário
            var roles = await _userManager.GetRolesAsync(user);

            // Impede a exclusão de qualquer usuário com a role Admin
            if (roles.Contains("Admin"))
            {
                deleteResults.Error = "Não é permitido excluir um usuário com a role Admin.";
                return deleteResults;
            }

            // Remove o usuário de todas as roles (se houver)
            if (roles.Any())
            {
                var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, roles);
                if (!removeRolesResult.Succeeded)
                {
                    deleteResults.Error = "Erro ao remover roles do usuário.";
                    return deleteResults;
                }
            }

            // Exclui o usuário
            var deleteResult = await _userManager.DeleteAsync(user);
            if (!deleteResult.Succeeded)
            {
                deleteResults.Error = "Não foi possível excluir o usuário.";
            }
            else
            {
                deleteResults.DeleteSucceded = true;
            }

            return deleteResults;
        }



        // ---------------------------------------------EDIT---------------------------------------------------
        public async Task<EditUserResults> EditAsync(EditUserViewModel model)
        {
            var result = new EditUserResults
            {
                Success = false
            };

            // Localiza o usuário pelo ID
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == model.Id);
            if (user == null)
            {
                result.Error = "Usuário não encontrado.";
                return result;
            }

            // Atualiza os campos do usuário
            user.UserName = model.UserName;
            user.Name = model.Name;
            user.IdNumber = model.IdNumber;
            user.Email = model.Email;

            // Atualiza o usuário no banco de dados
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                result.Error = "Erro ao atualizar o usuário.";
                return result;
            }


            result.Success = true;
            return result;
        }

        // ---------------------------------------------GETALL---------------------------------------------------
        public async Task<List<AppUserDto>> GetAllAsync()
        {
            var usersDto = await _userManager.Users
                .Select(user => new AppUserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    IdNumber = user.IdNumber,
                    Username = user.UserName ?? "",
                    Email = user.Email
                })
                .ToListAsync();

            return usersDto;
        }


        // ---------------------------------------------GETBYID---------------------------------------------------
        public async Task<AppUser?> GetByIdAsync(Guid id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return null;
            return user;
        }

        public async Task<List<string>> GetRolesAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity.IsAuthenticated)
                return new List<string>(); // Retorna lista vazia se não estiver autenticado

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return new List<string>();

            var identityUser = await _userManager.FindByIdAsync(userId);
            if (identityUser == null)
                return new List<string>();

            var roles = await _userManager.GetRolesAsync(identityUser);
            return roles.ToList();
        }


        // ---------------------------------------------LOGIN---------------------------------------------------
        public async Task<LoginResults> LoginAsync(LoginViewModel loginViewModel)
        {
            await _signInManager.SignOutAsync();
            var result = await _signInManager.PasswordSignInAsync(
                loginViewModel.Username,
                loginViewModel.Password,
                loginViewModel.RememberMe,
                lockoutOnFailure: true
            );
            // Inicializa o usuário e as roles como nulos
            AppUser? user = null;
            List<string>? roles = null;

            // Se o login for bem-sucedido, busca o usuário e suas roles
            if (result.Succeeded)
            {
                user = await _userManager.FindByNameAsync(loginViewModel.Username);
            }
            if (user == null) return new LoginResults();
            roles = (await _userManager.GetRolesAsync(user)).ToList();
            return new LoginResults
            {
                User = user,
                Roles = roles,
                Succeeded = result.Succeeded,
                IsLockedOut = result.IsLockedOut,
                IsNotAllowed = result.IsNotAllowed,
                RequiresTwoFactor = result.RequiresTwoFactor,
                Error = !result.Succeeded ? "Falha ao tentar realizar o login." : null
            };
        }

        // ---------------------------------------------LOGOUT---------------------------------------------------
        public async Task<bool> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return true;
        }

        // ---------------------------------------------REGISTER---------------------------------------------------
        public async Task<RegisterResults> RegisterAsync(RegisterViewModel model)
        {
            var registerResults = new RegisterResults();
            var user = new AppUser
            {
                UserName = model.UserName,
                Name = model.Name,
                IdNumber = model.IdNumber,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var resultRole = await _userManager.AddToRoleAsync(user, "Admin");
                registerResults.SucceededRole = resultRole.Succeeded;
                if (resultRole.Succeeded)
                {
                    // Confirmar o usuário automaticamente
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await _userManager.ConfirmEmailAsync(user, token);
                    // Logar
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
            }
            registerResults.Succeeded = result.Succeeded;
            return registerResults;
        }
    }
}