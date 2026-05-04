using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using ComicReader.DTOs;
using ComicReader.Services;
using FluentValidation;

namespace ComicReader.Controllers;

public class AccountController : Controller
{
    private readonly ICustomerService _customerService;
    private readonly IValidator<RegisterDto> _registerValidator;
    private readonly IValidator<LoginDto> _loginValidator;

    public AccountController(
        ICustomerService customerService,
        IValidator<RegisterDto> registerValidator,
        IValidator<LoginDto> loginValidator)
    {
        _customerService = customerService;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
    }

    [AllowAnonymous]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        var validation = await _registerValidator.ValidateAsync(registerDto);
        if (!validation.IsValid)
        {
            foreach (var error in validation.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return View(registerDto);
        }

        var result = await _customerService.RegisterAsync(registerDto);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View(registerDto);
        }

        TempData["SuccessMessage"] = "Registration successful. Please login.";
        return RedirectToAction(nameof(Login));
    }

    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginDto loginDto, string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        var validation = await _loginValidator.ValidateAsync(loginDto);
        if (!validation.IsValid)
        {
            foreach (var error in validation.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return View(loginDto);
        }

        var result = await _customerService.LoginAsync(loginDto);
        if (!result.Success || result.Data == null)
        {
            TempData["ErrorMessage"] = result.Message;
            return View(loginDto);
        }

        // Create claims and sign in
        var user = result.Data;
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("IsSubscribed", user.IsSubscribed.ToString()),
            new Claim("SubscribeEndDate", user.SubscribeEndDate?.ToString("o") ?? "")
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            new AuthenticationProperties { IsPersistent = loginDto.RememberMe, ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7) });

        TempData["SuccessMessage"] = $"Welcome back, {user.Name}!";
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["SuccessMessage"] = "Logged out successfully.";
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out int userId))
            return RedirectToAction(nameof(Login));

        var result = await _customerService.GetByIdAsync(userId);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction("Index", "Home");
        }
        return View(result.Data);
    }

    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }
}