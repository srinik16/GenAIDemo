# GenAIDemo

•	Implementation is extended to return a confidence score alongside the summary. The service now returns a SummaryResult containing Summary and a nullable Confidence (0..1).
•	securePromptingService attempts to extract confidence from typical response fields (e.g., choices[0].confidence, message.confidence) and falls back to a heuristic computed from token log-probs when available.
•	The HomeController exposes the value as ViewBag.ConfidenceScore (or "N/A" when not available). Format as a percentage for display if desired.
•	Note: many LLM endpoints do not provide an explicit confidence; when absent the heuristic is only an estimate — treat it as informational, not authoritative.
