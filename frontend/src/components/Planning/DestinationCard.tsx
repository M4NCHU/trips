import { FC } from "react";
import CardTitle from "./CardTitle";
import { Destination, DestinationCategory } from "../../types/Destination";
import { BsDot, BsThreeDotsVertical } from "react-icons/bs";
import { TripDestinationInterface } from "../../types/TripDestinationTypes";
import Card from "./Card";
import VisitPlaceCard from "./VisitPlaceCard";

interface DestinationCardProps {
  data: Destination;
}

const DestinationCard: FC<DestinationCardProps> = ({ data }) => {
  return (
    <div className="mb-4">
      <div className="flex items-center justify-between mb-4">
        <div className="flex flex-row gap-2 text-2xl items-center">
          <h2 className="font-bold">{data.name}</h2>
          <span className="text-gray-600 text-2xl">
            <BsDot />
          </span>
          <p className="font-semibold">
            {data.price} zł{" "}
            <span className="text-gray-600 text-base">/ person</span>
          </p>
        </div>

        <div></div>
      </div>
      <div className=" destination-reservation flex flex-row gap-4 justify-center">
        <div className="relative w-full ">
          <img
            src={data.photoUrl}
            alt={`${data.name}`}
            className="w-full  object-cover rounded-xl h-[12rem]"
          />
        </div>
      </div>
    </div>
  );
};

export default DestinationCard;
