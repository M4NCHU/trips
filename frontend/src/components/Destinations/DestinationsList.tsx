import { FC } from "react";
import DestinationItem from "./DestinationItem";
import { useQuery } from "@tanstack/react-query";
import { Destinations } from "@/src/types/Destinations";
import axios from "axios";

interface DestinationsListProps {}

const DestinationsList: FC<DestinationsListProps> = ({}) => {
  const destinations = useQuery({
    queryKey: ["destinations"],
    queryFn: async () => {
      const { data } = await axios.get(
        "https://localhost:7154/api/Destinations"
      );

      return data as Destinations[];
    },
  });

  return (
    <div className="categories-list flex flex-row flex-wrap mt-4 justify-center ">
      {destinations
        ? destinations.data?.map((item, i) => (
            <DestinationItem key={i} data={item} />
          ))
        : "no data"}
    </div>
  );
};

export default DestinationsList;
