namespace ITNOteAPI.Models;

public class NoteDto
{
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int ParentFolderId { get; set; }
    public string ParentFolderName { get; set; } = string.Empty;
}