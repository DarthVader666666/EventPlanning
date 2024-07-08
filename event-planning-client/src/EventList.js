import { Link } from 'react-router-dom';

const EventList = ({ events }) => {
  return (
    <div className="event-list">
      { events.map((event, index) => (
        <div className="event-preview" key={index} >
          <Link to={`/events/${event.eventId}`}>
            <h2>{ event.title }</h2>
          </Link>
        </div>
      ))}
    </div>
  );
}
 
export default EventList;