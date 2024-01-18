import { FC } from "react";
import CardTitle from "./CardTitle";
import { DestinationCategory } from "../../types/Destination";
import { BsThreeDotsVertical } from "react-icons/bs";
import { TripDestinationInterface } from "../../types/TripDestinationTypes";
import Card from "./Card";
import VisitPlaceCard from "./VisitPlaceCard";

interface DestinationCardProps {
  data: TripDestinationInterface;
}

const DestinationCard: FC<DestinationCardProps> = ({ data }) => {
  return (
    <div className="flex ">
      <CardTitle
        image={data.destination.photoUrl}
        alt={data.destination.name}
        title={data.destination.name}
      />
    </div>
  );
};

export default DestinationCard;
