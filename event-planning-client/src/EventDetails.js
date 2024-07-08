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
        <article>
          <h2>{ event.title }</h2>
          <p>Written by { event.location }</p>
          <div>{ event.date }</div>
        </article>
      )}

      {<button onClick={() => handleParticipate()}>Participate</button>}
    </div>
  );
}

export default EventDetails;