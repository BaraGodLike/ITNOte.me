namespace ITNOte.me.Model.Notes;

public class NoteDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int ParentFolderId { get; set; }
    public string ParentFolderName { get; set; } = string.Empty;
}