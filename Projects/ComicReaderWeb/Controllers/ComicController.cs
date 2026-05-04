using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ComicReader.Services;
using System.Security.Claims;

namespace ComicReader.Controllers;

[Authorize]
public class ComicController : Controller
{
    private readonly IComicService _comicService;
    private readonly ICustomerService _customerService;

    public ComicController(IComicService comicService, ICustomerService customerService)
    {
        _comicService = comicService;
        _customerService = customerService;
    }

    public async Task<IActionResult> Read(int id, int? chapterId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out int userId))
            return Challenge();

        // Get current customer to check subscription
        var customerResult = await _customerService.GetByIdAsync(userId);
        if (!customerResult.Success || customerResult.Data == null)
            return Challenge();

        bool isSubscribed = customerResult.Data.IsSubscribed &&
            (customerResult.Data.SubscribeEndDate > DateTime.UtcNow);

        var result = await _comicService.GetComicForReadingAsync(id, chapterId, userId, isSubscribed);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction("Index", "Home");
        }

        return View(result.Data);
    }
}