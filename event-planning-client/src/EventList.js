import { Link } from 'react-router-dom';

const EventList = ({ events }) => {
  return (
    <div className="event-list">
      { events.map(event => (
        <div className="event-preview" key={event.eventId} >
          <Link to={`/events/${event.eventId}`}>
            <h2>{ event.title }</h2>
          </Link>
        </div>
      ))}
    </div>
  );
}
 
export default EventList;