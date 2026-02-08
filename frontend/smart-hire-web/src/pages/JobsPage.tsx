import { useEffect, useState} from "react";
import type {FormEvent} from "react";
import { createJob, deleteJob, getJobs } from "../api/jobs";
import type { JobDto } from "../api/types";

const emptyJob: JobDto = {
  title: "",
  company: "",
  location: "",
  description: "",
  skillsRequired: [],
};

const JobsPage = () => {
  const [jobs, setJobs] = useState<JobDto[]>([]);
  const [form, setForm] = useState<JobDto>(emptyJob);
  const [skillsInput, setSkillsInput] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  async function loadJobs() {
    try {
      setLoading(true);
      setError(null);
      const data = await getJobs();
      setJobs(data);
    } catch (err: any) {
      setError(err.message || "Failed to load jobs");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadJobs();
  }, []);

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    try {
      setError(null);

      const jobToCreate: JobDto = {
        ...form,
        skillsRequired: skillsInput
          .split(",")
          .map((s) => s.trim())
          .filter(Boolean),
      };

      await createJob(jobToCreate);
      setForm(emptyJob);
      setSkillsInput("");
      await loadJobs();
    } catch (err: any) {
      setError(err.message || "Failed to create job");
    }
  }

  async function handleDelete(id?: string) {
    if (!id) return;
    if (!confirm("Delete this job?")) return;
    try {
      await deleteJob(id);
      await loadJobs();
    } catch (err: any) {
      setError(err.message || "Failed to delete job");
    }
  }

  return (
    <div className="page">
      <h1>Jobs</h1>

      {error && <div className="alert error">{error}</div>}

      <section className="card">
        <h2>Add Job</h2>
        <form className="form" onSubmit={handleSubmit}>
          <div className="form-row">
            <label>Title</label>
            <input
              value={form.title}
              onChange={(e) => setForm({ ...form, title: e.target.value })}
              required
            />
          </div>
          <div className="form-row">
            <label>Company</label>
            <input
              value={form.company}
              onChange={(e) => setForm({ ...form, company: e.target.value })}
              required
            />
          </div>
          <div className="form-row">
            <label>Location</label>
            <input
              value={form.location}
              onChange={(e) => setForm({ ...form, location: e.target.value })}
              required
            />
          </div>
          <div className="form-row">
            <label>Description</label>
            <textarea
              value={form.description}
              onChange={(e) => setForm({ ...form, description: e.target.value })}
              rows={3}
            />
          </div>
          <div className="form-row">
            <label>Skills required (comma separated)</label>
            <input
              value={skillsInput}
              onChange={(e) => setSkillsInput(e.target.value)}
              placeholder="React, .NET, MongoDB"
            />
          </div>
          <button type="submit" className="btn primary">
            Save Job
          </button>
        </form>
      </section>

      <section className="card">
        <h2>All Jobs</h2>
        {loading ? (
          <p>Loading...</p>
        ) : jobs.length === 0 ? (
          <p>No jobs yet.</p>
        ) : (
          <table className="table">
            <thead>
              <tr>
                <th>Title</th>
                <th>Company</th>
                <th>Location</th>
                <th>Skills</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {jobs.map((j) => (
                <tr key={j.id}>
                  <td>{j.title}</td>
                  <td>{j.company}</td>
                  <td>{j.location}</td>
                  <td>{j.skillsRequired?.join(", ")}</td>
                  <td>
                    <button className="btn danger" onClick={() => handleDelete(j.id)}>
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </section>
    </div>
  );
};

export default JobsPage;
