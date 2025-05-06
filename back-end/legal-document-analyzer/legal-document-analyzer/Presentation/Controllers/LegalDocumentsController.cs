using legal_document_analyzer.Domain.Entities;
using legal_document_analyzer.Domain.Repositories;
using legal_document_analyzer.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using UglyToad.PdfPig;
using static UglyToad.PdfPig.Core.PdfSubpath;

namespace legal_document_analyzer.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LegalDocumentsController : ControllerBase
    {
        private readonly ILegalDocumentRepository _repository;
        private readonly IClauseRepository clauseRepository;
        private readonly IDocumentSummaryRepository documentSummaryRepository;
        private readonly ILegalDocumentParser legalDocumentParser;

        public LegalDocumentsController(ILegalDocumentRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("upload")]
        [RequestSizeLimit(10 * 1024 * 1024)] // 10 MB limit
        public async Task<IActionResult> UploadDocument(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            if (file.ContentType != "application/pdf")
                return BadRequest("Only PDF files are allowed.");
            Console.WriteLine("Received file");
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();
            var content = ExtractTextFromPdf(fileBytes);
            Console.WriteLine("Extracted content");

            var document = new LegalDocument
            {
                LegalDocumentId = Guid.NewGuid(),
                FileName = file.FileName,
                Content = content,
                UploadedAt = DateTime.UtcNow
            };
            
            var (clauses, summary) =await _repository.AddDocumentAsync(document);
            var clauseContents = new List<string>();
            foreach (Clause clause in clauses)
                clauseContents.Add(clause.Text);
            /*
            var clauses = legalDocumentParser.ParseClauses(content, document.LegalDocumentId);
            foreach (var clause in clauses)
                await clauseRepository.AddClauseAsync(clause);
            var summary =await legalDocumentParser.GenerateSummary(content, document.LegalDocumentId);
            await documentSummaryRepository.AddSummaryAsync(summary);
            */
            return Ok(new { document.LegalDocumentId, document.FileName , clauseContents, summary.Content});
        }

        // Replace this with a real PDF text extraction
        private string ExtractTextFromPdf(byte[] pdfBytes)
        {
            using var pdfStream = new MemoryStream(pdfBytes);
            using var pdfDocument = PdfDocument.Open(pdfStream);
            var text = new System.Text.StringBuilder();

            foreach (var page in pdfDocument.GetPages())
            {
                text.Append(page.Text);
            }

            return text.ToString();
        }
    }
}
