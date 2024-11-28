using ITNOte.me.Model.Notes;
using ITNOte.me.Model.Storage;
using ITNOte.me.Model.User;
using ITNOteAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ITNOteAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IStorage storage) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserDto userDto)
    {
        var user = new User
        {
            Name = userDto.Name,
            Password = userDto.Password,
            GeneralFolder = new Folder(userDto.Name)
        };
        await storage.SaveUser(user);
        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserDto userDto)
    {
        var user = await storage.GetUserFromStorage<User>(userDto.Name);
        if (user == null || user.Password != userDto.Password)
            return Unauthorized("Invalid credentials.");

        return Ok(new { Message = "Login successful.", User = user });
    }
}

