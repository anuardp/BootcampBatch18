using Microsoft.AspNetCore.Mvc;
using ComicReader.Models;
using ComicReader.Services;

namespace ComicReaderWeb.Controllers;

public class HomeController : Controller
{
    private readonly IComicService _comicService;

    public HomeController(IComicService comicService)
    {
        _comicService = comicService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _comicService.GetAllComicsAsync();
        if (!result.Success)
            ViewBag.Error = result.Message;
        return View(result.Data ?? new List<Comic>());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Error()
    {
        return View();
    }
}
