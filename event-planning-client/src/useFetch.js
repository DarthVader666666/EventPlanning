import { useState, useEffect } from 'react';

const useFetch = (url) => {
  const [data, setData] = useState(null);
  const [isPending, setIsPending] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const abortCont = new AbortController();
    const token = sessionStorage.getItem("access_token");

    setTimeout(() => {
      fetch(url, 
      { 
        signal: abortCont.signal,
        headers:
        {
          "Accept": "application/json",
          "Authorization": "Bearer " + token
        }
      })
      .then(res => {
        if(res.status === 401)
        {
          sessionStorage.clear();
          throw Error('You are not authorized.');
        }

        if (!res.ok) { // error coming back from server
          throw Error('could not fetch the data for that resource');
        }       

        return res.json();
      })
      .then(data => {
        setIsPending(false);
        setData(data);
        setError(null);
      })
      .catch(err => {
        if (err.name === 'AbortError') {
          console.log('fetch aborted')
        } else {
          // auto catches network / connection error
          setIsPending(false);
          setError(err.message);
        }
      })
    }, 200);

    // abort the fetch
    return () => abortCont.abort();
  }, [url])

  return { data, isPending, error };
}

export default useFetch;