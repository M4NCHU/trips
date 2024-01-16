import { useEffect, useState } from "react";
import CategoriesList from "../../components/Categories/CategoriesList";
import DestinationsList from "../../components/Destinations/DestinationsList";
import { Link } from "react-router-dom";
import { useQuery } from "@tanstack/react-query";
import { Destination } from "@/src/types/Destination";
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
  return (
    <>
      <CategoriesList />
      <div className="container my-5 px-4">
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
