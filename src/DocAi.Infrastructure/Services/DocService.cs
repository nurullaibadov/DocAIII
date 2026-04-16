using DocAi.Core.Entities;
using DocAi.Core.Interfaces;

namespace DocAi.Infrastructure.Services;

public class DocService : IDocService
{
    private readonly IUnitOfWork _unitOfWork;

    public DocService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> UploadDocumentAsync(int userId, string fileName, string filePath)
    {
        var doc = new Document { FileName = fileName, FilePath = filePath, UserId = userId };
        await _unitOfWork.Repository<Document>().AddAsync(doc);
        await _unitOfWork.CommitAsync();
        return true;
    }

    public async Task<Document?> GetDocumentAsync(int id) =>
        await _unitOfWork.Repository<Document>().GetByIdAsync(id);

    public async Task<IEnumerable<Document>> GetUserDocumentsAsync(int userId) =>
        await _unitOfWork.Repository<Document>().Where(x => x.UserId == userId);

    public async Task<string> GenerateAiSummaryAsync(int documentId)
    {
        var doc = await _unitOfWork.Repository<Document>().GetByIdAsync(documentId);
        if (doc == null) throw new Exception("Document not found");

        await Task.Delay(1000);
        doc.AiSummary = $"AI Ozet: {doc.FileName} dosyasi analiz edildi.";

        await _unitOfWork.Repository<Document>().UpdateAsync(doc);
        await _unitOfWork.CommitAsync();
        return doc.AiSummary;
    }
}
