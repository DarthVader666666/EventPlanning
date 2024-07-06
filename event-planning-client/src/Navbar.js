import { Link } from "react-router-dom";

const Navbar = () => {
  const name = sessionStorage.getItem("user_name");

  return (
    <nav className="navbar">
      <h1>The Best Event Planning App</h1>
      <div className="links">
        <Link to="/">Home</Link>
        <Link to="/create" className="create-button">New Event</Link>
        <Link to="/login">{name ? name : "Log In"}</Link>
      </div>
    </nav>
  );
}
 
export default Navbar;