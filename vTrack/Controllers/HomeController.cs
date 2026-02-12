using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using vTrack.Models;
using vTrack.Models.ViewModels;
using vTrack.Services;

namespace vTrack.Controllers;

public class HomeController : Controller
{
    private PackageService _service;

    public HomeController(PackageService service)
    {
        _service = service;
    }


    public async Task<IActionResult> Index()
    {
        Package package = await _service.GetLatestPackageAsync();

        if(package == null)
        {
            package = new Package(0, 0, 0, DateTime.Now, " ");
        }

        IndexViewModel viewModel = new IndexViewModel(package);
        return View(viewModel);
    }


}
