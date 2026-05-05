using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ComicReader.Services;
using ComicReader.Models;
using ComicReader.DTOs;
using FluentValidation;
using System.Security.Claims;

namespace ComicReader.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IComicService _comicService;
    private readonly IChapterService _chapterService;
    private readonly IPageService _pageService;
    private readonly IValidator<AddNewComicDto> _addComicValidator;
    private readonly IValidator<AddNewChapterDto> _addChapterValidator;
    private readonly IValidator<AddPageDto> _addPageValidator;
    private readonly IValidator<UpdatePageDto> _updatePageValidator;

    public AdminController(
        IComicService comicService,
        IChapterService chapterService,
        IPageService pageService,
        IValidator<AddNewComicDto> addComicValidator,
        IValidator<AddNewChapterDto> addChapterValidator,
        IValidator<AddPageDto> addPageValidator,
        IValidator<UpdatePageDto> updatePageValidator)
    {
        _comicService = comicService;
        _chapterService = chapterService;
        _pageService = pageService;
        _addComicValidator = addComicValidator;
        _addChapterValidator = addChapterValidator;
        _addPageValidator = addPageValidator;
        _updatePageValidator = updatePageValidator;
    }

    // Dashboard
    public IActionResult Index()
    {
        return View();
    }

    // COMIC MANAGEMENT
    public async Task<IActionResult> Comics()
    {
        var result = await _comicService.GetAllComicsAsync();
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View(new List<Comic>()); // Kirim list kosong
        }
        return View(result.Data ?? new List<Comic>());
    }

    [HttpGet]
    public IActionResult AddComic()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddComic(AddNewComicDto dto)
    {
        var validation = await _addComicValidator.ValidateAsync(dto);
        if (!validation.IsValid)
        {
            foreach (var error in validation.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return View(dto);
        }

        var result = await _comicService.AddComicAsync(dto, User.FindFirst(ClaimTypes.Role)?.Value ?? "");
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View(dto);
        }

        TempData["SuccessMessage"] = "Comic added successfully.";
        return RedirectToAction(nameof(Comics));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveComic(RemoveComicDto dto)
    {
        var result = await _comicService.RemoveComicAsync(dto, User.FindFirst(ClaimTypes.Role)?.Value ?? "");
        if (!result.Success)
            TempData["ErrorMessage"] = result.Message;
        else
            TempData["SuccessMessage"] = "Comic removed.";
        return RedirectToAction(nameof(Comics));
    }

    // CHAPTER MANAGEMENT
    [HttpGet]
    public IActionResult AddChapter(int comicId)
    {
        var dto = new AddNewChapterDto { ComicId = comicId };
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddChapter(AddNewChapterDto dto)
    {
        var validation = await _addChapterValidator.ValidateAsync(dto);
        if (!validation.IsValid)
        {
            foreach (var error in validation.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            ViewBag.ComicId = dto.ComicId;
            return View(dto);
        }

        var result = await _chapterService.AddChapterAsync(dto, User.FindFirst(ClaimTypes.Role)?.Value ?? "");
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            ViewBag.ComicId = dto.ComicId;
            return View(dto);
        }

        TempData["SuccessMessage"] = $"Chapter {dto.ChapterNumber} added.";
        return RedirectToAction("EditComic", new { id = dto.ComicId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveChapter(RemoveChapterDto dto)
    {
        var chapterResult = await _chapterService.GetByIdAsync(dto.ChapterId);
        if (!chapterResult.Success || chapterResult.Data == null)
        {
            TempData["ErrorMessage"] = "Chapter not found.";
            return RedirectToAction("Comics");
        }

        var comicId = chapterResult.Data.ComicId;
        
        var result = await _chapterService.RemoveChapterAsync(dto, User.FindFirst(ClaimTypes.Role)?.Value ?? "");
        if (!result.Success)
            TempData["ErrorMessage"] = result.Message;
        else
            TempData["SuccessMessage"] = "Chapter removed.";

        return RedirectToAction("EditComic", new { id = comicId });
    }

    // PAGE MANAGEMENT
    [HttpGet]
public async Task<IActionResult> ManagePages(int chapterId)
{
    var chapterResult = await _chapterService.GetByIdAsync(chapterId);
    if (!chapterResult.Success || chapterResult.Data == null)
    {
        TempData["ErrorMessage"] = "Chapter not found.";
        return RedirectToAction("Comics");
    }
    
    ViewBag.ChapterNumber = chapterResult.Data.ChapterNumber;
    ViewBag.ChapterId = chapterId; 
    var pagesResult = await _pageService.GetPagesByChapterIdAsync(chapterId);
    return View(pagesResult.Data ?? new List<Page>());
}

    [HttpGet]
    public async Task<IActionResult> AddPage(int chapterId)
    {
        var chapterResult = await _chapterService.GetByIdAsync(chapterId);
        if (!chapterResult.Success || chapterResult.Data == null)
        {
            TempData["ErrorMessage"] = "Chapter not found.";
            return RedirectToAction("Comics");
        }
        
        ViewBag.ChapterNumber = chapterResult.Data.ChapterNumber;
        var dto = new AddPageDto { ChapterId = chapterId };
        return View(dto);
    }
    

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddPage(AddPageDto dto)
    {
        var validation = await _addPageValidator.ValidateAsync(dto);
        if (!validation.IsValid)
        {
            foreach (var error in validation.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            ViewBag.ChapterId = dto.ChapterId;
            return View(dto);
        }

        var result = await _pageService.AddPageAsync(dto, User.FindFirst(ClaimTypes.Role)?.Value ?? "");
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            ViewBag.ChapterId = dto.ChapterId;
            return View(dto);
        }

        TempData["SuccessMessage"] = $"Page {dto.PageNumber} added.";
        return RedirectToAction(nameof(ManagePages), new { chapterId = dto.ChapterId });
    }

    [HttpGet]
    public async Task<IActionResult> EditPage(int id)
    {
        var pageResult = await _pageService.GetByIdAsync(id);
        if (!pageResult.Success || pageResult.Data == null)
        {
            TempData["ErrorMessage"] = "Page not found.";
            return RedirectToAction("Index");
        }
        var dto = new UpdatePageDto
        {
            Id = pageResult.Data.Id,
            PageNumber = pageResult.Data.PageNumber,
            PageUrl = pageResult.Data.PageUrl
        };
        return View(dto);
    }
    public async Task<IActionResult> EditComic(int id)
    {
        var comicResult = await _comicService.GetByIdWithChaptersAsync(id); 
        if (!comicResult.Success || comicResult.Data == null)
        {
            TempData["ErrorMessage"] = "Comic not found.";
            return RedirectToAction(nameof(Comics));
        }
        return View(comicResult.Data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPage(UpdatePageDto dto)
    {
        var validation = await _updatePageValidator.ValidateAsync(dto);
        if (!validation.IsValid)
        {
            foreach (var error in validation.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return View(dto);
        }

        var result = await _pageService.UpdatePageAsync(dto, User.FindFirst(ClaimTypes.Role)?.Value ?? "");
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View(dto);
        }

        TempData["SuccessMessage"] = "Page updated.";
        return RedirectToAction(nameof(ManagePages), new { chapterId = result.Data?.ChapterId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemovePage(RemovePageDto dto)
    {
        // get chapterId first
        var pageResult = await _pageService.GetByIdAsync(dto.PageId);
        if (!pageResult.Success || pageResult.Data == null)
        {
            TempData["ErrorMessage"] = "Page not found.";
            return RedirectToAction("Comics");
        }
        
        var chapterId = pageResult.Data.ChapterId;
        
        var result = await _pageService.RemovePageAsync(dto, User.FindFirst(ClaimTypes.Role)?.Value ?? "");
        if (!result.Success)
            TempData["ErrorMessage"] = result.Message;
        else
            TempData["SuccessMessage"] = "Page removed.";
        
        return RedirectToAction("ManagePages", new { chapterId = chapterId });
    }
}