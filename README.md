# GenAIDemo
Secure AI Summarization:

Implements a secure AI summarization flow: HomeController calls securePromptingService to submit a user prompt to an external chat-completion endpoint, first sanitizing PII (emails and phone numbers), validating prompt length, and awaiting the async response. The service posts a safe prompt, parses the JSON response, performs a final sensitive-data check, and returns a summary (exposed to the view via ViewBag.Summary). Designed for debugging with explicit error surfacing; replace the placeholder API key/endpoint and move HttpClient to DI (Dependency Injection) for production use.
