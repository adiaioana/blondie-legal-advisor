using Azure;
using Azure.AI.TextAnalytics;
using legal_document_analyzer.Domain.Entities;
using legal_document_analyzer.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;



namespace legal_document_analyzer.Infrastructure.Services
{
    public class LegalDocumentParser : ILegalDocumentParser
    {
        private readonly TextAnalyticsClient _textAnalyticsClient;

        public LegalDocumentParser(string azureEndpoint, string azureApiKey)
        {
            var credentials = new AzureKeyCredential(azureApiKey);
            _textAnalyticsClient = new TextAnalyticsClient(new Uri(azureEndpoint), credentials);
        }

        public IEnumerable<Clause> ParseClauses(string content, Guid documentId)
        {
            const int MaxSize = 5120;
            var clauses = new List<Clause>();

            for (int i = 0; i < content.Length; i += MaxSize)
            {
                string chunk = content.Substring(i, Math.Min(MaxSize, content.Length - i));
                var response = _textAnalyticsClient.RecognizeEntities(chunk);

                foreach (var entity in response.Value)
                {
                    Console.WriteLine(entity.Category);
                    if (entity.Category == "LegalInformation")
                    {
                        clauses.Add(new Clause
                        {
                            ClauseId = Guid.NewGuid(),
                            DocumentId = documentId, 
                            Type = ClauseType.General,
                            Text = entity.Text,
                            Explanation = $"Category: {entity.Category}, Confidence: {entity.ConfidenceScore}"
                        });
                    }
                }
            }

            return clauses;
        }

        public async Task<DocumentSummary> GenerateSummary(string content, Guid documentId)
        {
            var batchInput = new List<string>
            {
                content
            };
            TextAnalyticsActions actions = new TextAnalyticsActions()
            {
                ExtractiveSummarizeActions = new List<ExtractiveSummarizeAction>() { new ExtractiveSummarizeAction() }
            };

            // Start analysis process.
            AnalyzeActionsOperation operation = await _textAnalyticsClient.StartAnalyzeActionsAsync(batchInput, actions);
            await operation.WaitForCompletionAsync();
            // Use Text Analytics Extractive Summarization

            var summarySentences = new List<string>();
            await foreach (AnalyzeActionsResult documentsInPage in operation.Value)
            {
                IReadOnlyCollection<ExtractiveSummarizeActionResult> summaryResults = documentsInPage.ExtractiveSummarizeResults;

                foreach (ExtractiveSummarizeActionResult summaryActionResults in summaryResults)
                {
                    if (summaryActionResults.HasError)
                    {
                        Console.WriteLine($"  Error!");
                        Console.WriteLine($"  Action error code: {summaryActionResults.Error.ErrorCode}.");
                        Console.WriteLine($"  Message: {summaryActionResults.Error.Message}");
                        continue;
                    }

                    foreach (ExtractiveSummarizeResult documentResults in summaryActionResults.DocumentsResults)
                    {
                        if (documentResults.HasError)
                        {
                            Console.WriteLine($"  Error!");
                            Console.WriteLine($"  Document error code: {documentResults.Error.ErrorCode}.");
                            Console.WriteLine($"  Message: {documentResults.Error.Message}");
                            continue;
                        }

                        Console.WriteLine($"  Extracted the following {documentResults.Sentences.Count} sentence(s):");
                        Console.WriteLine();

                        foreach (ExtractiveSummarySentence sentence in documentResults.Sentences)
                        {
                            Console.WriteLine($"  Sentence: {sentence.Text}");
                            summarySentences.Add(sentence.Text);
                            Console.WriteLine();
                        }
                    }
                }
            }
            return new DocumentSummary
            {
                DocumentSummaryId = Guid.NewGuid(),
                DocumentId =documentId, 
                Style = SummaryStyle.General,
                Content = string.Join(" ", summarySentences) 
            };
        }
    }
}