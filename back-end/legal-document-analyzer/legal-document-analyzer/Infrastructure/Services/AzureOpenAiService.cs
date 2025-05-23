using Azure;
using Azure.AI.OpenAI;
using legal_document_analyzer.Domain.Entities;
using System.Text.Json;

namespace legal_document_analyzer.Infrastructure.Services
{
    public class AzureOpenAiService : IAzureOpenAiService
    {
        private readonly IConfiguration _config;
        private readonly OpenAIClient _openAiClient;
        private readonly int _maxtokens;
        private readonly string _deployment;

        public AzureOpenAiService(IConfiguration config)
        {
            _config = config;

            var endpoint = _config["OpenAI4oMiniModel:Endpoint2"];
            var key = _config["OpenAI4oMiniModel:ApiKey"];
            _deployment = _config["OpenAI4oMiniModel:Model"];
            _maxtokens = int.Parse(_config["OpenAI4oMiniModel:Max_Tokens"]);

            var credentials = new AzureKeyCredential(key);
            _openAiClient = new OpenAIClient(new Uri(endpoint), credentials);
        }

        public async Task<string> GetCompletionAsync(List<MyChatMessage> chatHistory)
        {
            // Map your MyChatMessage objects to Azure OpenAI ChatMessage format
            var messages = chatHistory.Select(chatMessage =>
                new Azure.AI.OpenAI.ChatMessage(
                    chatMessage.Sender == "user" ? ChatRole.User : ChatRole.Assistant,
                    chatMessage.Content
                )
            ).ToList();

            // Create the ChatCompletionsOptions object
            var options = new Azure.AI.OpenAI.ChatCompletionsOptions
            {
                MaxTokens = _maxtokens
            };

            // Add messages to the options
            foreach (var message in messages)
            {
                options.Messages.Add(message);
            }

            try
            {
                // Request a chat completion from Azure OpenAI
                var response = await _openAiClient.GetChatCompletionsAsync(_deployment, options);
                var completion = response.Value.Choices.FirstOrDefault()?.Message.Content;

                // Return the completion text
                return completion ?? string.Empty;
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log errors, rethrow, or return a fallback response)
                Console.WriteLine($"Error generating chat completion: {ex.Message}");
                throw;
            }
        }
    }
}