using System.Diagnostics;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using FM20;

namespace App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FeatureManager _featureManager;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _featureManager = new FeatureManager();
            
            // Initialize some default features
            _featureManager.EnableFeature("WelcomeMessage");
            _featureManager.EnableFeature("UserDashboard");
            _featureManager.DisableFeature("BetaFeatures");
        }

        public IActionResult Index()
        {
            ViewBag.WelcomeEnabled = _featureManager.IsFeatureEnabled("WelcomeMessage");
            ViewBag.FM20Version = FeatureManager.GetVersion();
            ViewBag.EnabledFeatures = _featureManager.GetEnabledFeatures().ToList();
            
            _logger.LogInformation("Home page accessed. FM20 version: {Version}", FeatureManager.GetVersion());
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Features()
        {
            var model = new
            {
                EnabledFeatures = _featureManager.GetEnabledFeatures().ToList(),
                Version = FeatureManager.GetVersion()
            };
            
            return Json(model);
        }
    }
}
