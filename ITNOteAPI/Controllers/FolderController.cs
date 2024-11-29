using ITNOte.me.Model.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITNOteAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FolderController(IStorage storage) : ControllerBase
{
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAllChildren(int id)
    {
        return Ok(await storage.GetAllChildren(id));
    }
}