import { FC } from "react";
import CardTitle from "./CardTitle";
import { Destination, DestinationCategory } from "../../types/Destination";
import { BsThreeDotsVertical } from "react-icons/bs";
import { TripDestinationInterface } from "../../types/TripDestinationTypes";
import Card from "./Card";
import VisitPlaceCard from "./VisitPlaceCard";

interface DestinationCardProps {
  data: Destination;
}

const DestinationCard: FC<DestinationCardProps> = ({ data }) => {
  return (
    <div className="flex ">
      <CardTitle
        image={data.photoUrl}
        alt={data.name}
        title={data.name}
        link={`/destination/${data.id}`}
      />
    </div>
  );
};

export default DestinationCard;
