using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Http.Json;

namespace GenAIDemo.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            ViewBag.Title = "Home Page";

            using (HttpClient client = new HttpClient())
            {
                // Set up the API key for authentication
                string apiKey = "xxxxxx-xxxx-xxxx-xxxx-xxxx412fb472";

                client.DefaultRequestHeaders.Add("api-key", apiKey);

                securePromptingService secure = new securePromptingService(client);

                string prompt = "Provide a summary of the following text: John Doe's email is john@wbm.com and his phone number is 202-456-3242. He works at Contoso Ltd.";
                try
                {
                    // Await the async operation to get the summary
                    var summaryTask = await secure.GetSecureSummaryAsync(prompt);
                    ViewBag.Summary = summaryTask;
                }
                catch(Exception ex)
                {
                    ViewBag.Summary = "Error: " + ex.Message;
                }
            }
            return View();
        }
    }

    public class securePromptingService
    {
        private readonly HttpClient _httpClient;
        public securePromptingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // Constructor logic here
        }

        // Method to get secure summary
        public async Task<string> GetSecureSummaryAsync(string prompt)
        {
            // Step.1 Validate prompt length
            if (string.IsNullOrEmpty(prompt) || prompt.Length>1000)
                throw new ArgumentException("Invalid prompt");

            // Step.2 Redact sensitive information from prompt
            string sanitizedPrompt = SanitizePromt(prompt);

            // Step.3 Construct safe prompt template
            string safePrompt = $"Summarize the following text without exposing any confidential detail: {sanitizedPrompt}";

            // Step.4 Call OpenAI API with safe prompt
            string apiVerion = "2024-12-01-preview";
            string deploymentName = "gpt-4.1";

            var payload = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "system", content = "You are a compliance-safe summarizer." },
                    new { role = "user", content = safePrompt }
                }
            };
            // Step.5 Process response and ensure no sensitive data is returned
            var response = await _httpClient.PostAsJsonAsync($"https://abc.com/api/v1/subscription/openai/deployments/{deploymentName}/chat/completions?api-version={apiVerion}", payload);

            // Ensure the response is successful
            var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>();

            string aiouput = string.Empty;

            // Extract the content from the response
            if (jsonResponse.ValueKind == JsonValueKind.Object && jsonResponse.TryGetProperty("choices", out JsonElement choicesElement) && choicesElement.GetArrayLength() > 0 && choicesElement[0].TryGetProperty("message", out JsonElement messageElement) && messageElement.TryGetProperty("content", out JsonElement contentElement))
            {
                aiouput = contentElement.ValueKind == JsonValueKind.String ? contentElement.GetString() : string.Empty;
            }

            // Final check for sensitive information in the output
            if (IsSensitive(aiouput))
            {
                throw new Exception("Response contains sensitive information");
            }

            // Return the sanitized AI output
            return aiouput;
        }

        // Method to check for sensitive information
        private bool IsSensitive(string aiouput)
        {
            return Regex.IsMatch(aiouput, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}") || Regex.IsMatch(aiouput, @"\b\d{3}[-.]?\d{3}[-.]?\d{4}\b");
        }

        // Method to sanitize prompt
        private string SanitizePromt(string prompt)
        {
            //regex to replace email with [REDACTED]
            prompt = Regex.Replace(prompt, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}", "abc@gmail.com");
            //regex to replace phone number with [REDACTED]
            prompt = Regex.Replace(prompt, @"\b\d{3}[-.]?\d{3}[-.]?\d{4}\b", "123-456-7890");
            //return sanitized prompt
            return prompt;
        }
    }
}
