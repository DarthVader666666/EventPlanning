import { Link } from 'react-router-dom';

const EventList = ({ events }) => {
  return (
    <div className="event-list">
      { events.map(event => (
        <div className="event-preview" key={event.eventid} >
          <Link to={`/events/${event.eventid}`}>
            <h2>{ event.title }</h2>
          </Link>
        </div>
      ))}
    </div>
  );
}
 
export default EventList;