using Microsoft.AspNetCore.Mvc;
using UserProfileManager.Models;

namespace UserProfileManager.Controllers;

public class ErrorController : Controller
{
  public IActionResult ErrorPopup(string exception)
  {
    var errorViewModel = new ErrorViewModel
    {
      RequestId = HttpContext.TraceIdentifier,
      ErrorMessage = exception
    };

    return View("ErrorPopup", errorViewModel);
  }
}

