import { apiGet, apiPost, apiPut, apiDelete } from "./client";
import type { CandidateDto } from "./types";

export async function getCandidates(): Promise<CandidateDto[]> {
  return apiGet<CandidateDto[]>("/api/candidates");
}

export async function getCandidate(id: string): Promise<CandidateDto> {
  return apiGet<CandidateDto>(`/api/candidates/${id}`);
}

export async function createCandidate(candidate: CandidateDto): Promise<CandidateDto> {
  return apiPost<CandidateDto, CandidateDto>("/api/candidates", candidate);
}

export async function updateCandidate(id: string, candidate: CandidateDto): Promise<CandidateDto> {
  return apiPut<CandidateDto, CandidateDto>(`/api/candidates/${id}`, candidate);
}

export async function deleteCandidate(id: string): Promise<void> {
  return apiDelete(`/api/candidates/${id}`);
}
