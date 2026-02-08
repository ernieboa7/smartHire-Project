import { NavLink } from "react-router-dom";

const NavBar = () => {
  return (
    <header className="navbar">
      <div className="navbar-brand">SmartHire</div>
      <nav className="navbar-links">
        <NavLink
          to="/candidates"
          className={({ isActive }) => (isActive ? "nav-link active" : "nav-link")}
        >
          Candidates
        </NavLink>
        <NavLink
          to="/jobs"
          className={({ isActive }) => (isActive ? "nav-link active" : "nav-link")}
        >
          Jobs
        </NavLink>
        <NavLink
          to="/assistant"
          className={({ isActive }) => (isActive ? "nav-link active" : "nav-link")}
        >
          AI Assistant
        </NavLink>
      </nav>
    </header>
  );
};

export default NavBar;
