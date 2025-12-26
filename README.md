# GenAI Demo
Secure AI Summarization with Confidence Scoring

This demo project demonstrates how to build a secure, responsible AI-powered text summarization flow using a server-side architecture. The focus is not just on generating summaries, but on doing so safely, with guardrails around sensitive data and clear visibility into how the AI response is produced.

At a high level, the application accepts a user’s input text, performs basic security checks, and then sends a sanitized prompt to an external AI chat-completion endpoint. Before the request is made, common PII such as email addresses and phone numbers are masked, and the prompt length is validated to prevent abuse or malformed requests. The AI-generated summary is then returned and displayed back to the user.

The flow is intentionally kept transparent and debuggable. Errors are surfaced clearly during development so the full request/response lifecycle can be understood. For production use, the API key and endpoint should be externalized, and HttpClient should be registered through dependency injection rather than instantiated directly.

<img width="1183" height="384" alt="image" src="https://github.com/user-attachments/assets/bef8ff32-9dc6-4f88-99c5-338247825f57" />

<img width="757" height="572" alt="image" src="https://github.com/user-attachments/assets/7a63ed99-80b9-4a95-bb0c-a95069ed86ba" />

<img width="673" height="322" alt="image" src="https://github.com/user-attachments/assets/575c9367-4db3-48f6-a213-0d29f5aef3d4" />

<img width="565" height="357" alt="image" src="https://github.com/user-attachments/assets/8980f051-7ca1-4418-b773-ebd26236a3ec" />

