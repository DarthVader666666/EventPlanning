import { useNavigate, useParams } from "react-router";
import useFetch from "./useFetch";
import { useState } from "react";

const EventDetails = () => {
  const { eventId } = useParams();
  const serverBaseUrl = process.env.REACT_APP_API_URL;
  const { data: event } = useFetch(`${serverBaseUrl}/events/` + eventId);
  const [isPending, setPending] = useState(false);
  const navigate = useNavigate();
  const email = sessionStorage.getItem('user_name');

  const handleParticipate = () =>
  {
    const token = sessionStorage.getItem('access_token');

    if(email === null)
    {
        navigate("/login");
        return;
    }

    setPending(true);

    fetch(`${serverBaseUrl}/events/participate/`, 
    {
      method: "POST",
      headers: 
            {
              "Content-Type": "application/json",
              "Authorization": "Bearer " + token
            },
      body: JSON.stringify({ eventId, email })
    }).then(() => setPending(false)).then(() => alert('Confirmation link sent. Please, check your email!'))
      .then(() => navigate("/"));
  }

  return (
    <div className="event-details">
      {isPending && (<div><h3>Sending confirmation on email... </h3><span>{email}</span></div>)}
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