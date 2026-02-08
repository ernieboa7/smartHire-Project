import { useState } from "react";
import type {FormEvent} from "react";
import { sendChatMessage } from "../api/chat";
import type { ChatMessageDto, ChatResponseDto } from "../api/types";

interface ChatMessageView extends ChatMessageDto {
  id: string;
}

const ChatPage = () => {
  const [sessionId, setSessionId] = useState<string | null>(null);
  const [messages, setMessages] = useState<ChatMessageView[]>([]);
  const [input, setInput] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  function addMessagesFromResponse(res: ChatResponseDto) {
    // Map backend messages into local format with ids
    const mapped: ChatMessageView[] = res.messages.map((m, index) => ({
      ...m,
      id: `${res.sessionId}-${index}-${m.timestampUtc}`,
    }));
    setMessages(mapped);
  }

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    if (!input.trim()) return;

    try {
      setLoading(true);
      setError(null);

      const res = await sendChatMessage({
        sessionId,
        message: input,
      });

      setSessionId(res.sessionId);
      addMessagesFromResponse(res);
      setInput("");
    } catch (err: any) {
      setError(err.message || "Failed to send message");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="page chat-page">
      <h1>AI Assistant</h1>
      {sessionId && <p className="session-id">Session: {sessionId}</p>}
      {error && <div className="alert error">{error}</div>}

      <section className="card chat-card">
        <div className="chat-messages">
          {messages.length === 0 ? (
            <p className="chat-placeholder">Ask the assistant about candidates, jobs, or hiring tips.</p>
          ) : (
            messages.map((m) => (
              <div
                key={m.id}
                className={
                  m.role === "user" ? "chat-message user" : "chat-message assistant"
                }
              >
                <div className="chat-role">{m.role}</div>
                <div className="chat-content">{m.content}</div>
              </div>
            ))
          )}
        </div>

        <form className="chat-input-row" onSubmit={handleSubmit}>
          <input
            className="chat-input"
            placeholder="Type a message..."
            value={input}
            onChange={(e) => setInput(e.target.value)}
            disabled={loading}
          />
          <button type="submit" className="btn primary" disabled={loading}>
            {loading ? "Sending..." : "Send"}
          </button>
        </form>
      </section>
    </div>
  );
};

export default ChatPage;
