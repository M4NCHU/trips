import { useEffect, useState } from "react";

type DestinationType = {
  id: number;
  name: string;
  description: string;
  location: string;
  photoUrl: string;
};

const Home = () => {
  const [destinations, setDestinations] = useState<DestinationType[]>([]);

  useEffect(() => {
    fetch("https://localhost:7154/api/Destinations")
      .then((res) => {
        return res.json();
      })
      .then((data) => {
        setDestinations(data);
        console.log(data);
      });
  }, []);

  return (
    <div>
      <h1>Destinations: </h1>
      {destinations
        ? destinations.map((item, i) => <p key={i}>{item.name}</p>)
        : "No data"}
    </div>
  );
};

export default Home;
