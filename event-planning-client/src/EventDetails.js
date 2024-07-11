import { useNavigate, useParams } from "react-router";
import useFetch from "./useFetch";

const EventDetails = () => {
  const { eventId } = useParams();
  const serverBaseUrl = process.env.REACT_APP_API_URL;
  const { data: event, error, isPending } = useFetch(`${serverBaseUrl}/events/` + eventId);
  const navigate = useNavigate();

  const handleParticipate = () =>
  {
    const email = sessionStorage.getItem('user_name');
    const token = sessionStorage.getItem('access_token');

    if(email === null)
    {
        console.log("Please log in first.");
        navigate("/login");
        return;
    }

    fetch(`${serverBaseUrl}/events/participate/`, 
    {
      method: "POST",
      headers: 
            {
              "Content-Type": "application/json",
              "Authorization": "Bearer " + token
            },
      body: JSON.stringify({ eventId, email })
    }).then(() => navigate("/"));
  }

  return (
    <div className="event-details">
      { isPending && <div>Loading...</div> }
      { error && <div>{ error }</div> }
      { event && (
        <div>
          <h2>{ event.title }</h2>
          <h3>Location:</h3>
          <div>{ event.location }</div>
          <h3>Date:</h3>
          <div>{ event.date }</div>
          <h3>Participants:</h3>       
          <div>{ event.participants }</div>
        </div>
      )}

      { <button onClick={() => handleParticipate()}>Participate</button> }
    </div>
  );
}

export default EventDetails;