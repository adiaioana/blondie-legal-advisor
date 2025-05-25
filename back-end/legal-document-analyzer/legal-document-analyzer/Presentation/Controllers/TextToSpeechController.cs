using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace legal_document_analyzer.Presentation.Controllers
{
    [ApiController]
    [Route("api/tts")]
    public class TextToSpeechController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public TextToSpeechController(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ConversationRequest request)
        {
            var subscriptionKey = _config["AzureSpeech:Key"];
            var region = _config["AzureSpeech:Region"];
            var uri = $"https://{region}.tts.speech.microsoft.com/cognitiveservices/v1";

            var results = new List<AudioResponse>();

            foreach (var pair in request.Conversation)
            {
                // Question (use voice 1)
                var questionSsml = $@"
        <speak version='1.0' xml:lang='en-US'>
          <voice name='en-US-JennyNeural'>{pair.Question}</voice>
        </speak>";
                var questionReq = new HttpRequestMessage(HttpMethod.Post, uri);
                questionReq.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                questionReq.Headers.Add("User-Agent", "LegalDocAnalyzer");
                questionReq.Content = new StringContent(questionSsml, Encoding.UTF8, "application/ssml+xml");
                questionReq.Headers.Add("X-Microsoft-OutputFormat", "audio-16khz-32kbitrate-mono-mp3");

                var questionRes = await _httpClient.SendAsync(questionReq);
                if (!questionRes.IsSuccessStatusCode)
                    return StatusCode((int)questionRes.StatusCode, "TTS failed for question");

                var questionAudio = await questionRes.Content.ReadAsByteArrayAsync();
                results.Add(new AudioResponse
                {
                    Type = "question",
                    Audio = Convert.ToBase64String(questionAudio)
                });

                // Answer (use voice 2)
                var answerSsml = $@"
        <speak version='1.0' xml:lang='en-US'>
          <voice name='en-US-GuyNeural'>{pair.Answer}</voice>
        </speak>";
                var answerReq = new HttpRequestMessage(HttpMethod.Post, uri);
                answerReq.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                answerReq.Headers.Add("User-Agent", "LegalDocAnalyzer");
                answerReq.Content = new StringContent(answerSsml, Encoding.UTF8, "application/ssml+xml");
                answerReq.Headers.Add("X-Microsoft-OutputFormat", "audio-16khz-32kbitrate-mono-mp3");

                var answerRes = await _httpClient.SendAsync(answerReq);
                if (!answerRes.IsSuccessStatusCode)
                    return StatusCode((int)answerRes.StatusCode, "TTS failed for answer");

                var answerAudio = await answerRes.Content.ReadAsByteArrayAsync();
                results.Add(new AudioResponse
                {
                    Type = "answer",
                    Audio = Convert.ToBase64String(answerAudio)
                });
            }

            return Ok(results);
        }

        public class ConversationRequest
        {
            public List<ConversationPair> Conversation { get; set; }
        }

        public class ConversationPair
        {
            public string Question { get; set; }
            public string Answer { get; set; }
        }

        public class AudioResponse
        {
            public string Type { get; set; } // "question" or "answer"
            public string Audio { get; set; } // base64-encoded mp3
        }
    }
}
