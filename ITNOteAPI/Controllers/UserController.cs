﻿using ITNOte.me.Model.Notes;
using ITNOte.me.Model.Storage;
using ITNOte.me.Model.User;
using Microsoft.AspNetCore.Mvc;

namespace ITNOteAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IStorage storage) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(string name, string password)
    {
        var user = new User
        {
            Name = name,
            Password = Storage.Hasher.HashPassword(password),
            GeneralFolder = new Folder(name)
        };
        await storage.SaveUser(user);
        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserDto userDto)
    {
        var user = await storage.GetUserFromStorage<User>(userDto.Name);
        if (user == null || !Storage.Hasher.VerifyHashedPassword(userDto.Password, user.Password))
            return Unauthorized("Invalid credentials.");
        var token = TokenService.GenerateToken(userDto.Name, "User");
        return Ok(new { token });
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetUser(string name)
    {
        if (!await storage.HasNicknameInStorage(name)) return Unauthorized();
        var user = await storage.GetUserFromStorage<User>(name);
        return Ok(user);
    }
}

