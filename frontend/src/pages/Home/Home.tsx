import { useEffect, useState } from "react";
import CategoriesList from "../../components/Categories/CategoriesList";
import DestinationsList from "../../components/Destinations/DestinationsList";
import { Link } from "react-router-dom";
import { useQuery } from "@tanstack/react-query";
import { Destinations } from "@/src/types/Destinations";
import axios from "axios";
import { GoPlus } from "react-icons/go";

type DestinationType = {
  id: number;
  name: string;
  description: string;
  location: string;
  photoUrl: string;
};

const Home = () => {
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
      <div className="container my-5">
        <div className="flex justify-end fixed bottom-4 right-4 z-[9999]">
          <Link
            to="/destinations/create"
            className="bg-secondary p-5 text-xl font-bold rounded-full"
          >
            <GoPlus />
          </Link>
        </div>
        <DestinationsList />
      </div>
    </>
  );
};

export default Home;
