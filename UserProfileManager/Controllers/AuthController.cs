using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using UserProfileManager.Business.Interfaces;
using UserProfileManager.DTOs;
using UserProfileManager.Models;

namespace UserProfileManager.Controllers;

[Route("[controller]")]
public class AuthController : Controller
{
  private readonly IUserBusiness _userBusiness;
  private readonly IMemoryCache _memoryCache;

  public AuthController(IUserBusiness userBusiness, IMemoryCache memoryCache)
  {
    _userBusiness = userBusiness;
    _memoryCache = memoryCache;
  }

  [HttpGet("Login")]
  public IActionResult Login()
  {
    return View();
  }

  [HttpPost("Login")]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Login(LoginViewDTO model)
  {
    try
    {
      if (ModelState.IsValid)
      {
        User result = await _userBusiness.LoginAsync(model.UniqueName, model.Password);
        if (result.Id != 0)
        {
          List<Claim> claims = new()
                        {
                            new Claim(ClaimTypes.Name, model.UniqueName)
                        };

          ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
          ClaimsPrincipal principal = new(identity);

          await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

          string fullName = $"{result.FirstName} {result.MiddleName} {result.LastName}";
          _memoryCache.Set("LoggedInUserFullName", fullName);

          return RedirectToAction("Index", "User");
        }
        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
      }
      return View(model);
    }
    catch (Exception ex)
    {
      var errorDetails = new ErrorViewModel
      {
        ErrorMessage = ex.Message,
        RequestId = HttpContext.TraceIdentifier
      };

      return View("ErrorPopup", errorDetails);
    }
  }

  [HttpGet("Logout")]
  public async Task<IActionResult> Logout()
  {
    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    _memoryCache.Remove("LoggedInUserFullName");
    return RedirectToAction("Login");
  }

  [HttpGet("AccessDenied")]
  public IActionResult AccessDenied()
  {
    return View();
  }

  [HttpGet("ErrorPopup")]
  public IActionResult ErrorPopup()
  {
    var errorDetails = new ErrorViewModel
    {
      RequestId = TempData["RequestId"] as string,
      ErrorMessage = TempData["ErrorMessage"] as string ?? string.Empty
    };
    return View("ErrorPopup", errorDetails);
  }
}


