﻿using ITNOte.me.Model.Notes;
using ITNOte.me.Model.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITNOteAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotesController(IStorage storage) : ControllerBase
{
    [Authorize]
    [HttpGet("protected/{id}")]
    public async Task<IActionResult> GetNoteContent(int id, string name)
    {
        var content = await storage.ReadNote(id, name);
        return Ok(new { Id = id, Name = name, Content = content });
    }
    
    [Authorize]
    [HttpPost("protected")]
    public async Task<IActionResult> CreateNote([FromBody] NoteDto noteDto)
    {
        var note = new Note(noteDto.Name)
        {
            Content = noteDto.Content,
            Parent = new Folder(noteDto.ParentFolderName) { Id = noteDto.ParentFolderId }
        };
        await storage.CreateNewSource(note);
        return CreatedAtAction(nameof(GetNoteContent), new { id = note.Id, name = note.Name }, note);
    }
    
    [Authorize]
    [HttpPut("protected/{id}")]
    public async Task<IActionResult> UpdateNoteContent(int id, string name, string newContent)
    {
        await storage.WriteInNote(id, name, newContent);
        return NoContent();
    }
    
    [Authorize]
    [HttpDelete("protected/{id}")]
    public async Task<IActionResult> DeleteNoteContent(int id)
    {
        await storage.DeleteNote(id);
        return NoContent();
    }
}
