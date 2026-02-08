import { apiGet, apiPost, apiPut, apiDelete } from "./client";
import type { JobDto } from "./types";

export async function getJobs(): Promise<JobDto[]> {
  return apiGet<JobDto[]>("/api/jobs");
}

export async function getJob(id: string): Promise<JobDto> {
  return apiGet<JobDto>(`/api/jobs/${id}`);
}

export async function createJob(job: JobDto): Promise<JobDto> {
  return apiPost<JobDto, JobDto>("/api/jobs", job);
}

export async function updateJob(id: string, job: JobDto): Promise<JobDto> {
  return apiPut<JobDto, JobDto>(`/api/jobs/${id}`, job);
}

export async function deleteJob(id: string): Promise<void> {
  return apiDelete(`/api/jobs/${id}`);
}
