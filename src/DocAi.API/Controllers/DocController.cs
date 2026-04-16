using DocAi.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DocAi.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DocController : ControllerBase
{
    private readonly IDocService _docService;
    public DocController(IDocService docService) => _docService = docService;

    private int GetUserId() =>
        int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException("User ID not found in token"));

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("File is empty");
        var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        Directory.CreateDirectory(uploadsDir);
        var path = Path.Combine(uploadsDir, file.FileName);
        using (var stream = new FileStream(path, FileMode.Create))
            await file.CopyToAsync(stream);
        await _docService.UploadDocumentAsync(GetUserId(), file.FileName, path);
        return Ok("Document uploaded");
    }

    [HttpGet]
    public async Task<IActionResult> GetMyDocs() =>
        Ok(await _docService.GetUserDocumentsAsync(GetUserId()));

    [HttpPost("ai-summary/{id}")]
    public async Task<IActionResult> GenerateSummary(int id) =>
        Ok(await _docService.GenerateAiSummaryAsync(id));
}
