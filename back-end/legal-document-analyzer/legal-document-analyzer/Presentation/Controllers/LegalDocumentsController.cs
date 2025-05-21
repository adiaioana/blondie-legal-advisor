using legal_document_analyzer.Application.Services;
using legal_document_analyzer.Domain.Entities;
using legal_document_analyzer.Domain.Repositories;
using legal_document_analyzer.Domain.ValueObjects;
using legal_document_analyzer.Infrastructure.Extensions;
using legal_document_analyzer.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;
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
        private readonly ITokenService _tokenService;
        private readonly IDocumentSummaryRepository documentSummaryRepository;
        private readonly ILegalDocumentParser legalDocumentParser;

        public LegalDocumentsController(ILegalDocumentRepository repository)
        {
            _repository = repository;
        }

        private string DecodeToken(TokenRequest tokenRequest)
        {

            if (tokenRequest == null || string.IsNullOrEmpty(tokenRequest.RefreshToken))
            {
                return "Invalid token";
            }

            var token = Request.GetBearerToken();

            if (string.IsNullOrEmpty(token))
            {
                return "Invalid token";
            }


            var principal = _tokenService.GetPrincipalFromExpiredToken(token);
            if (principal == null)
            {
                return "Invalid token";
            }
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return "User ID claim not found in token";
            }
            var userId =userIdClaim.Value.ToString();
            return userId;


        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllDocuments()
        {
            var userIdStringOrNot = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdStringOrNot))
                return Unauthorized("User ID claim not found in token");

            if (!Guid.TryParse(userIdStringOrNot, out var userId))
                return Unauthorized("Invalid user id format");

            var documents = await _repository.GetDocumentsByUserIdAsync(userId);
            return Ok(documents);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocument(string id)
        {

            var userIdStringOrNot = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdStringOrNot))
                return Unauthorized("User ID claim not found in token");

            if (!Guid.TryParse(userIdStringOrNot, out var userId))
                return Unauthorized("Invalid user id format");
            var document = await _repository.GetDocumentByIdAsync(Guid.Parse(id));

            if (document == null)
                return NotFound();

            if (document.UserId != userId)
                return Unauthorized();

            return Ok(document);
        }

        [Authorize]
        [HttpPost("upload")]
        [RequestSizeLimit(10 * 1024 * 1024)] // 10 MB limit
        public async Task<IActionResult> UploadDocument(IFormFile file)
        {

            var userIdStringOrNot = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdStringOrNot))
                return Unauthorized("User ID claim not found in token");

            if (!Guid.TryParse(userIdStringOrNot, out var userId))
                return Unauthorized("Invalid user id format");

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
                UserId = userId,
                FileName = file.FileName,
                Content = content,
                UploadedAt = DateTime.UtcNow
            };
            
            var (clauses, summary) =await _repository.AddDocumentAsync(document);
            var clauseContents = new List<string>();
            foreach (Clause clause in clauses)
                clauseContents.Add(clause.Text);
            
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
