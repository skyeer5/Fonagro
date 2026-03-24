using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Application.Authentication.Command.Login;
using WebApp.Persistence.Models;
using static WebApp.Application.Authentication.Command.Login.LoginCommand;

namespace WebApp.Web.Controllers;

public class AuthController : Controller
{
    private readonly IMediator _mediator;
    private readonly SignInManager<AppUser> _signInManager;

    public AuthController(IMediator mediator, SignInManager<AppUser> signInManager)
    {
        _mediator = mediator;
        _signInManager = signInManager;

    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(
        [FromForm] LoginRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new LoginCommandRequest(request);
        var resultado = await _mediator.Send(command, cancellationToken);
        if(!resultado.IsSuccess)
        {
            TempData["msg"] = resultado.Error!;
            return View(request);
        }
        return RedirectToAction("Index", "Home");
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Auth");
    }
}