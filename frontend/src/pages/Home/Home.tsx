import { useEffect, useState } from "react";
import CategoriesList from "../../components/Categories/CategoriesList";
import DestinationsList from "../../components/Destinations/DestinationsList";
import { Link } from "react-router-dom";

type DestinationType = {
  id: number;
  name: string;
  description: string;
  location: string;
  photoUrl: string;
};

const Home = () => {
  const [destinations, setDestinations] = useState<DestinationType[]>([]);

  // useEffect(() => {
  //   fetch("https://localhost:7154/api/Destinations")
  //     .then((res) => {
  //       return res.json();
  //     })
  //     .then((data) => {
  //       setDestinations(data);
  //       console.log(data);
  //     });
  // }, []);

  return (
    <>
      <CategoriesList />
      <Link to="/destinations/create">Create +</Link>
      <DestinationsList />
      {destinations
        ? destinations.map((item, i) => <p key={i}>{item.name}</p>)
        : "No data"}
    </>
  );
};

export default Home;
