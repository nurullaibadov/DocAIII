namespace DocAi.Core.Entities;

public class Document : BaseEntity
{
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string? AiSummary { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
