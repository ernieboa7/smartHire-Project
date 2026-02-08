// Candidate DTO – mirrors CandidateDto.cs
export interface CandidateDto {
  id?: string;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  skills: string[];
  experienceYears: number;
}

// Job DTO – mirrors JobDto.cs
export interface JobDto {
  id?: string;
  title: string;
  company: string;
  location: string;
  description: string;
  skillsRequired: string[];
}

// Chat DTOs – mirrors ChatRequestDto.cs / ChatResponseDto.cs
export interface ChatRequestDto {
  sessionId?: string | null;
  message: string;
  candidateId?: string | null;
  jobId?: string | null;
}

export interface ChatMessageDto {
  role: string; // "user" | "assistant" | "system"
  content: string;
  timestampUtc: string;
}

export interface ChatResponseDto {
  sessionId: string;
  assistantReply: string;
  messages: ChatMessageDto[];
}
