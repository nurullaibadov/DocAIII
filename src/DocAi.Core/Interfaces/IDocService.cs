using DocAi.Core.Entities;

namespace DocAi.Core.Interfaces;

public interface IDocService
{
    Task<bool> UploadDocumentAsync(int userId, string fileName, string filePath);
    Task<Document?> GetDocumentAsync(int id);
    Task<IEnumerable<Document>> GetUserDocumentsAsync(int userId);
    Task<string> GenerateAiSummaryAsync(int documentId);
}
