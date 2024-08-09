using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using UserProfileManager.Business.Interfaces;
using UserProfileManager.Models;

namespace UserProfileManager.Controllers;

[Route("[controller]")]
public class UserController : Controller
{
  private readonly IUserBusiness _userBusiness;
  private readonly IMemoryCache _memoryCache;
  public UserController(IUserBusiness userBusiness, IMemoryCache memoryCache)
  {
    _userBusiness = userBusiness;
    _memoryCache = memoryCache;
  }

  [Route("User/AccessDenied")]
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

  [HttpGet("Check/{uniqueName}")]
  public async Task<IActionResult> CheckUniqueName(string uniqueName)
  {
    User user = await _userBusiness.CheckUniqueName(uniqueName);
    if (user != null)
    {
      return Json(new { exists = true });
    }
    return Json(new { exists = false });
  }

  [HttpGet]
  [Authorize]
  public async Task<IActionResult> Index()
  {
    var users = await _userBusiness.GetAllAsync();

    string cachedUniqueName = _memoryCache.Get<string>("UniqueName") ?? string.Empty;

    ViewBag.CachedUniqueName = cachedUniqueName;

    return View(users);
  }

  [HttpGet("Details/{id}")]
  [Authorize]
  public async Task<IActionResult> Details(int id)
  {
    var user = await _userBusiness.GetByIdAsync(id);
    if (user == null)
    {
      return NotFound();
    }
    return View(user);
  }

  [HttpGet("Create")]
  public IActionResult Create()
  {
    return View();
  }

  [HttpPost("Create")]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Create([Bind("UniqueName,Password,FirstName,PhoneNumber,Gender,Email,DateOfBirth,MiddleName,LastName,Address,City,CountryCode")] User user, string[] Education, IFormFile? photo)
  {
    try
    {
      if (!ModelState.IsValid)
      {
        // Log model state errors
        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
        {
          Console.WriteLine(error.ErrorMessage);
        }
        return View(user);
      }

      if (photo != null && photo.Length > 0)
      {
        string[] allowedExtensions = new[] { ".jpeg", ".jpg", ".png" };
        string extension = Path.GetExtension(photo.FileName).ToLower();

        if (!allowedExtensions.Contains(extension))
        {
          ModelState.AddModelError("Photo", "Only .jpeg, .jpg, and .png files are allowed.");
          return View(user);
        }

        using (var memoryStream = new MemoryStream())
        {
          await photo.CopyToAsync(memoryStream);
          user.Photo = memoryStream.ToArray();
        }
      }
      else
      {
        user.Photo = null;
      }
      user.Education = string.Join(", ", Education);

      await _userBusiness.AddAsync(user);

      return RedirectToAction(nameof(Index));
    }
    catch (Exception ex)
    {
      TempData["ErrorMessage"] = ex.Message;
      return RedirectToAction("ErrorPopup");
    }
  }



  [HttpGet("Edit/{id}")]
  [Authorize]
  public async Task<IActionResult> Edit(int id)
  {
    var user = await _userBusiness.GetByIdAsync(id);
    if (user == null)
    {
      return NotFound();
    }
    return View(user);
  }

  [HttpPost("Edit/{id}")]
  [ValidateAntiForgeryToken]
  [Authorize]
  public async Task<IActionResult> Edit(int id, [Bind("Id,UUID,UniqueName,Password,FirstName,CountryCode,PhoneNumber,Gender,Email,DateOfBirth,CreatedOn,UpdatedOn,MiddleName,LastName,Address,City,Education")] User user, string[] Education, IFormFile? photo)
  {
    try
    {
      if (id != user.Id)
      {
        return BadRequest();
      }

      if (ModelState.IsValid)
      {
        if (photo != null)
        {
          string[] allowedExtensions = new[] { ".jpeg", ".jpg", ".png" };
          string extension = Path.GetExtension(photo.FileName).ToLower();

          if (!allowedExtensions.Contains(extension))
          {
            ModelState.AddModelError("Photo", "Only .jpeg, .jpg, and .png files are allowed.");
            return View(user);
          }

          using (var memoryStream = new MemoryStream())
          {
            await photo.CopyToAsync(memoryStream);
            user.Photo = memoryStream.ToArray();
          }
        }

        user.Education = string.Join(", ", Education);

        user.UpdatedOn = DateTime.UtcNow;

        await _userBusiness.UpdateAsync(user);
        return RedirectToAction(nameof(Index));
      }

      if (!ModelState.IsValid)
      {
        // Log detailed model state errors
        foreach (var modelState in ModelState)
        {
          Console.WriteLine($"Key: {modelState.Key}");
          foreach (var error in modelState.Value.Errors)
          {
            Console.WriteLine($"Error: {error.ErrorMessage}");
          }
        }
        return View(user);
      }

      return View(user);
    }
    catch (Exception ex)
    {
      TempData["ErrorMessage"] = ex.Message;
      return RedirectToAction("ErrorPopup");
    }
  }

  [HttpGet("Delete/{id}")]
  [Authorize]
  public async Task<IActionResult> Delete(int id)
  {
    var user = await _userBusiness.GetByIdAsync(id);
    if (user == null)
    {
      return NotFound();
    }
    return View(user);
  }

  [HttpPost("Delete/{id}"), ActionName("Delete")]
  [ValidateAntiForgeryToken]
  [Authorize]
  public async Task<IActionResult> DeleteConfirmed(int id)
  {
    try
    {
      var user = await _userBusiness.GetByIdAsync(id);
      if (user == null)
      {
        return NotFound();
      }

      await _userBusiness.DeleteAsync(id);
      return RedirectToAction(nameof(Index));
    }
    catch (Exception ex)
    {
      TempData["ErrorMessage"] = ex.Message;
      return RedirectToAction("ErrorPopup");
    }
  }
}
