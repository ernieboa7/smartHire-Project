

import { Routes, Route, Navigate } from "react-router-dom";
import CandidatesPage from "./pages/CandidatesPage";
import JobsPage from "./pages/JobsPage";
import ChatPage from "./pages/ChatPage";
import NavBar from "./components/NavBar";
import './App.css'

const App = () => {
  return (
    <div className="app-container">
      <NavBar />
      <main className="app-main">
        <Routes>
          <Route path="/" element={<Navigate to="/candidates" replace />} />
          <Route path="/candidates" element={<CandidatesPage />} />
          <Route path="/jobs" element={<JobsPage />} />
          <Route path="/assistant" element={<ChatPage />} />
        </Routes>
      </main>
    </div>
  );
};

export default App;

