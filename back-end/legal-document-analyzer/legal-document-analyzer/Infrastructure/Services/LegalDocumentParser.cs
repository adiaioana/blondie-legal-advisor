using Azure;
using legal_document_analyzer.Domain.Entities;
using legal_document_analyzer.Domain.ValueObjects;
using Azure.AI.Inference;
using System.Text.RegularExpressions;

namespace legal_document_analyzer.Infrastructure.Services
{
    public class LegalDocumentParser : ILegalDocumentParser
    {

        private Uri _modelEndpoint;
        private ChatCompletionsClient _chatClient;
        private string _model;
        private int _maxtokens;
        public LegalDocumentParser(string azureEndpoint, string azureApiKey, string modelName, int maxTokens)
        {
            var credentials = new AzureKeyCredential(azureApiKey);
            this._model = modelName;
            this._modelEndpoint = new Uri(azureEndpoint);
            this._chatClient = new ChatCompletionsClient(
                _modelEndpoint,
                credentials,
                new AzureAIInferenceClientOptions()
            );
            this._maxtokens = maxTokens;
        }

        public async Task<IEnumerable<Clause>> ParseClauses(string content, Guid documentId)
        {
            Console.WriteLine(content);
            var prompt = $"Extract all legal clauses from the following document. Return each clause as a numbered list:\n\n{content}";
            var requestOptions = new ChatCompletionsOptions()
            {
                   Messages =
                   {
                        new ChatRequestUserMessage(prompt)
                    },
                   MaxTokens = this._maxtokens,
                   Model = this._model
            };

            StreamingResponse<StreamingChatCompletionsUpdate> response = await this._chatClient.CompleteStreamingAsync(requestOptions);
            var lines = String.Empty;
            await foreach (StreamingChatCompletionsUpdate chatUpdate in response)
            {
                if (!string.IsNullOrEmpty(chatUpdate.ContentUpdate))
                {
                   // Console.WriteLine($"Type: {chatUpdate.ContentUpdate?.GetType()}");
                    ///Console.WriteLine($"Raw Response: {chatUpdate.ContentUpdate}");
                    var someLines = chatUpdate.ContentUpdate;
                  //  Console.WriteLine("Got clause: "+ someLines);
                    if (someLines.Length > 0)
                        lines += someLines;

                }
            }
            /*
            var requestBody = new
            {
                model = "deepseek-coder",
                messages = new[]
                {
                new { role = "user", content = prompt }
            },
                temperature = 0.5
            };

            var response = await this._httpClient.PostAsJsonAsync(_modelEndpoint, requestBody);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadFromJsonAsync<ChatResponse>();

            var lines = responseJson?.Choices[0]?.Message?.Content?.Split('\n') ?? Array.Empty<string>();
            */
            var clauses = new List<Clause>();
            Console.WriteLine($"Got lines:{lines}");
            string[] parts = Regex.Split(lines, @"\d+\.\s");
            Console.WriteLine($"Got parts:{parts}");

            foreach (var line in parts)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    clauses.Add(new Clause
                    {
                        ClauseId = Guid.NewGuid(),
                        DocumentId = documentId,
                        Type = ClauseType.General,
                        Text = line.Trim(),
                        Explanation = "Extracted by DeepSeek"
                    });
                }
            }

            return clauses;
        }

        public async Task<DocumentSummary> GenerateSummary(string content, Guid documentId)
        {
            var prompt = $"Summarize the following legal document:\n\n{content}";
            /*
            var requestBody = new
            {
                model = "deepseek-coder", // or your model name  
                messages = new[]
                {
                   new { role = "user", content = prompt }
               },
                temperature = 0.7
            };

            var response = await _httpClient.PostAsJsonAsync(_modelEndpoint, requestBody);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadFromJsonAsync<ChatResponse>();

            // Fix: Adjusted to use the 'Messages' property of ChatResponse instead of 'Choices'  */
            var requestOptions = new ChatCompletionsOptions()
            {
                Messages =
                   {
                        new ChatRequestUserMessage(prompt)
                    },
                MaxTokens = this._maxtokens,
                Model = this._model
            };

            StreamingResponse<StreamingChatCompletionsUpdate> response = await this._chatClient.CompleteStreamingAsync(requestOptions);
            var lines = String.Empty;
            await foreach (StreamingChatCompletionsUpdate chatUpdate in response)
            {
                if (!string.IsNullOrEmpty(chatUpdate.ContentUpdate))
                {
                   // Console.WriteLine($"Raw Response: {chatUpdate.ContentUpdate}");
                    var someLines = chatUpdate.ContentUpdate;
                    lines += someLines;
                }
            }
            Console.WriteLine("The summary is:\n", lines);
            return new DocumentSummary
            {
                DocumentSummaryId = Guid.NewGuid(),
                DocumentId = documentId,
                Style = SummaryStyle.General,
                Content = lines ?? "No summary generated."
            };
        }
    }
    /*
    public class ChatResponse
    {
        public Choice[] Choices { get; set; }

        public class Choice
        {
            public Message Message { get; set; }
        }

        public class Message
        {
            public string Role { get; set; }
            public string Content { get; set; }
        }
    }
    */
}