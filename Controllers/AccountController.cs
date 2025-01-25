using acadgest.Controllers;
using acadgest.Interface;
using acadgest.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


public class AccountController : Controller
{
    private readonly IAccountRepository _accountRepo;

    public AccountController(IAccountRepository accountRepo)
    {
        _accountRepo = accountRepo;
    }

    //--------------------------------------------EXIBINDO AS PÁGINAS--------------------------------------------
    // -------------------------------------Exibe a página de Login
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    // -------------------------------------Exibe a página de usuários
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var users = await _accountRepo.GeAllAsync();
        return View(users);
    }
    // -------------------------------------Exibe a página de registro
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    // -------------------------------------Exibe a página de edição
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var user = await _accountRepo.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound("Usuário não encontrado.");
        }

        var model = new EditUserViewModel
        {
            Id = user.Id,
            UserName = user.UserName!,
            Name = user.Name!,
            IdNumber = user.IdNumber!,
            Email = user.Email!,
            EmailConfirmed = user.EmailConfirmed
        };

        return View(model);
    }

    //--------------------------------------------PROCESSANDO AS REQUISIÇÕES---------------------------------------
    // -------------------------------------Processa o login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null!)
    {
        returnUrl ??= Url.Content("~/");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var loginResult = await _accountRepo.LoginAsync(model);

        if (!loginResult.Succeeded)
        {
            if (loginResult.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Sua conta está bloqueada. Tente novamente mais tarde.");
            }
            else if (loginResult.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "Login não permitido. Confirme sua conta antes de tentar novamente.");
            }
            else if (loginResult.RequiresTwoFactor)
            {
                ModelState.AddModelError(string.Empty, "É necessário autenticação de dois fatores.");
                // Aqui, você pode redirecionar para uma página de autenticação de dois fatores.
            }
            else
            {
                ModelState.AddModelError(string.Empty, loginResult.Error ?? "Erro desconhecido ao tentar realizar o login.");
            }

            return View(model);
        }
        return LocalRedirect(returnUrl);
    }



    // -------------------------------------Processa o registro
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _accountRepo.RegisterAsync(model);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Erro ao registrar usuário.");
            }
            else if (!result.SucceededRole)
            {
                ModelState.AddModelError(string.Empty, "Erro ao adicionar usuário à role.");
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
        return View(model);
    }

    // -------------------------------------Processa o logout
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        var logout = await _accountRepo.LogoutAsync();
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }



    // -------------------------------------Processa o edit
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(EditUserViewModel model)
    {

        var result = await _accountRepo.EditAsync(model);
        if (!result.Success) ModelState.AddModelError("", result.Error);
        return RedirectToAction("Index");
    }
    // -------------------------------------Processa o delete
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        var deletResult = await _accountRepo.DeletAsync(userId);
        if (!deletResult.DeleteSucceded) ModelState.AddModelError("", deletResult.Error);
        // Redireciona ou retorna sucesso
        return RedirectToAction("Index");
    }
    // -------------------------------------Acesso negado
    public IActionResult AccessDenied()
    {
        return View();
    }
}
