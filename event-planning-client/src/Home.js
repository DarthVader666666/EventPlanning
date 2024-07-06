import BlogList from "./EventList";
import useFetch from "./useFetch";

const Home = () => {
  const environmentName = process.env.REACT_APP_ENV;
  const serverBaseUrl = process.env.REACT_APP_API_URL;
  const { error, isPending, data: blogs } = useFetch(`${serverBaseUrl}/events/`)
  
    return (
    <div className="home">
      Environment: {environmentName}
      { error && <div>{ error }</div> }
      { isPending && <div>Loading...</div> }
      { blogs && <BlogList blogs={blogs} /> }
    </div>
  );
}
 
export default Home;
