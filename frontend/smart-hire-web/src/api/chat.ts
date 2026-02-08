import { apiPost } from "./client";
import type { ChatRequestDto, ChatResponseDto } from "./types";

export async function sendChatMessage(request: ChatRequestDto): Promise<ChatResponseDto> {
  return apiPost<ChatRequestDto, ChatResponseDto>("/api/aiassistant/chat", request);
}
