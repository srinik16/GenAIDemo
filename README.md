# GenAIDemo
Secure AI Summarization:

Implements a secure AI summarization flow: HomeController calls securePromptingService to submit a user prompt to an external chat-completion endpoint, first sanitizing PII (emails and phone numbers), validating prompt length, and awaiting the async response. The service posts a safe prompt, parses the JSON response, performs a final sensitive-data check, and returns a summary (exposed to the view via ViewBag.Summary). Designed for debugging with explicit error surfacing; replace the placeholder API key/endpoint and move HttpClient to DI (Dependency Injection) for production use. Below screenshot shows the input prompt and AI summarized output.

•	Implementation is extended (in GenAIwithConfidenceScore branch)  to return a confidence score alongside the summary. The service now returns a SummaryResult containing Summary and a nullable Confidence (0..1).
•	securePromptingService attempts to extract confidence from typical response fields (e.g., choices[0].confidence, message.confidence) and falls back to a heuristic computed from token log-probs when available.
•	The HomeController exposes the value as ViewBag.ConfidenceScore (or "N/A" when not available). Format as a percentage for display if desired.
•	Note: many LLM endpoints do not provide an explicit confidence; when absent the heuristic is only an estimate — treat it as informational, not authoritative.

<img width="1183" height="384" alt="image" src="https://github.com/user-attachments/assets/bef8ff32-9dc6-4f88-99c5-338247825f57" />

<img width="757" height="572" alt="image" src="https://github.com/user-attachments/assets/7a63ed99-80b9-4a95-bb0c-a95069ed86ba" />

<img width="673" height="322" alt="image" src="https://github.com/user-attachments/assets/575c9367-4db3-48f6-a213-0d29f5aef3d4" />

<img width="565" height="357" alt="image" src="https://github.com/user-attachments/assets/8980f051-7ca1-4418-b773-ebd26236a3ec" />

