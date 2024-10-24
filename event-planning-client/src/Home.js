import EventList from "./EventList";
import useFetch from "./useFetch";

const Home = () => {
  // const environmentName = process.env.REACT_APP_ENV;
  const serverBaseUrl = process.env.REACT_APP_API_URL;
  const { error, isPending, data: events } = useFetch(`${serverBaseUrl}/events/`)

  return (
    <div className="home">
      { error && <div>{ error }</div> }
      { isPending && <div>Loading...</div> }
      { events && <EventList events={(events)} /> }
    </div>
  );
}
 
export default Home;
