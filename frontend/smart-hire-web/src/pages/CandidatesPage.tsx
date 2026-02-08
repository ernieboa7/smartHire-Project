import { useEffect, useState } from "react";
import type {FormEvent} from "react";
import { createCandidate, deleteCandidate, getCandidates } from "../api/candidates";
import type { CandidateDto } from "../api/types";

const emptyCandidate: CandidateDto = {
  firstName: "",
  lastName: "",
  email: "",
  phone: "",
  skills: [],
  experienceYears: 0,
};

const CandidatesPage = () => {
  const [candidates, setCandidates] = useState<CandidateDto[]>([]);
  const [form, setForm] = useState<CandidateDto>(emptyCandidate);
  const [skillsInput, setSkillsInput] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  async function loadCandidates() {
    try {
      setLoading(true);
      setError(null);
      const data = await getCandidates();
      setCandidates(data);
    } catch (err: any) {
      setError(err.message || "Failed to load candidates");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadCandidates();
  }, []);

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    try {
      setError(null);

      const candidateToCreate: CandidateDto = {
        ...form,
        skills: skillsInput
          .split(",")
          .map((s) => s.trim())
          .filter(Boolean),
        experienceYears: Number(form.experienceYears) || 0,
      };

      await createCandidate(candidateToCreate);
      setForm(emptyCandidate);
      setSkillsInput("");
      await loadCandidates();
    } catch (err: any) {
      setError(err.message || "Failed to create candidate");
    }
  }

  async function handleDelete(id?: string) {
    if (!id) return;
    if (!confirm("Delete this candidate?")) return;
    try {
      await deleteCandidate(id);
      await loadCandidates();
    } catch (err: any) {
      setError(err.message || "Failed to delete candidate");
    }
  }

  return (
    <div className="page">
      <h1>Candidates</h1>

      {error && <div className="alert error">{error}</div>}

      <section className="card">
        <h2>Add Candidate</h2>
        <form className="form" onSubmit={handleSubmit}>
          <div className="form-row">
            <label>First name</label>
            <input
              value={form.firstName}
              onChange={(e) => setForm({ ...form, firstName: e.target.value })}
              required
            />
          </div>
          <div className="form-row">
            <label>Last name</label>
            <input
              value={form.lastName}
              onChange={(e) => setForm({ ...form, lastName: e.target.value })}
              required
            />
          </div>
          <div className="form-row">
            <label>Email</label>
            <input
              type="email"
              value={form.email}
              onChange={(e) => setForm({ ...form, email: e.target.value })}
              required
            />
          </div>
          <div className="form-row">
            <label>Phone</label>
            <input
              value={form.phone || ""}
              onChange={(e) => setForm({ ...form, phone: e.target.value })}
            />
          </div>
          <div className="form-row">
            <label>Skills (comma separated)</label>
            <input
              value={skillsInput}
              onChange={(e) => setSkillsInput(e.target.value)}
              placeholder="React, C#, MongoDB"
            />
          </div>
          <div className="form-row">
            <label>Experience (years)</label>
            <input
              type="number"
              min={0}
              value={form.experienceYears}
              onChange={(e) =>
                setForm({ ...form, experienceYears: Number(e.target.value) })
              }
            />
          </div>
          <button type="submit" className="btn primary">
            Save Candidate
          </button>
        </form>
      </section>

      <section className="card">
        <h2>All Candidates</h2>
        {loading ? (
          <p>Loading...</p>
        ) : candidates.length === 0 ? (
          <p>No candidates yet.</p>
        ) : (
          <table className="table">
            <thead>
              <tr>
                <th>Name</th>
                <th>Email</th>
                <th>Skills</th>
                <th>Experience</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {candidates.map((c) => (
                <tr key={c.id}>
                  <td>
                    {c.firstName} {c.lastName}
                  </td>
                  <td>{c.email}</td>
                  <td>{c.skills?.join(", ")}</td>
                  <td>{c.experienceYears}</td>
                  <td>
                    <button className="btn danger" onClick={() => handleDelete(c.id)}>
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

export default CandidatesPage;
