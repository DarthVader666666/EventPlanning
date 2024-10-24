import Navbar from './Navbar';
import Home from './Home';
import Create from './Create';
import LogIn from './Login';
import Register from './Register';
import EventDetails from './EventDetails';
import Confirm from './Confirm';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';

function App() {
  return (
    <Router>
      <div className="App">
        <Navbar />
        <div className="content">
          <Routes>
            <Route path="/" element={<Home/>}></Route>
            <Route path="/create" element={<Create/>}></Route>
            <Route path="/login/" element={<LogIn/>}></Route>
            <Route path="/register/" element={<Register/>}></Route>
            <Route path="/:eventId" element={<EventDetails />}></Route>
            <Route path="/confirm/:userId/:eventId" element={<Confirm />}></Route>
          </Routes>
        </div>
      </div>
    </Router>
  );
}

export default App;
