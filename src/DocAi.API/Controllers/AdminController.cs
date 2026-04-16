using DocAi.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocAi.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    public AdminController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _unitOfWork.Repository<DocAi.Core.Entities.User>().GetAllAsync();
        return Ok(users.Select(u => new { u.Id, u.FullName, u.Email, u.Role }));
    }
}
