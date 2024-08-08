using Microsoft.AspNetCore.Mvc;

namespace UserProfileManager.Controllers
{
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
      _logger = logger;
    }

    public IActionResult Index()
    {
      return Redirect("/Auth/Login");
    }

    public IActionResult Privacy()
    {
      return View();
    }
  }
}
