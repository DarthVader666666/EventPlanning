import { useNavigate, useParams } from "react-router";
import { useState } from "react";

const Confirm = () => {
  const { userId } = useParams();  
  const { eventId } = useParams();
  const serverBaseUrl = process.env.REACT_APP_API_URL;
  const navigate = useNavigate();
  const [status, setStatus] = useState(null);

  const email = sessionStorage.getItem('user_name');
  const token = sessionStorage.getItem('access_token');

  fetch(`${serverBaseUrl}/events/confirm/${userId}/${eventId}`, 
    {
      method: "GET",
      headers: 
            {
              "Content-Type": "application/json",
              "Authorization": "Bearer " + token
            }
    }).then(response => setStatus(response.status));

  return (
    <div className="confirm">
        {
            status === 200 ?
            <article>
              <h2>Thank You! { email }</h2>
              <p>Confirmation approved!</p>
            </article> :
            <h2>Something went wrong :(</h2>
        }
        {<button onClick={() => navigate("/")}>Home</button>}
    </div>
  );
}

export default Confirm;