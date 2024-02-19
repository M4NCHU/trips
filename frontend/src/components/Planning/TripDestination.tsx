import { useDestinationById } from "src/api/Destinations";
import DestinationCard from "./DestinationCard";
import VisitPlaceCard from "./VisitPlaceCard";
import { useVisitPlacesByDestination } from "src/api/VisitPlaceAPI";
import { TripDestination } from "src/types/TripDestinationTypes";
import { SelectedPlace } from "src/types/SelectedPlaceTypes";
import { FC } from "react";
import Card from "./Card";

interface TripDestinationData {
  data: TripDestination;
}

const TripDestinationComponent: FC<TripDestinationData> = ({ data }) => {
  if (!data) return null;
  return (
    <>
      <div className="flex flex-col border-b-2 pb-6">
        <div className="mb-4">
          <h1 className="text-2xl font-semibold">Day 1</h1>
        </div>
        <DestinationCard data={data.destination} />
        <div className=" px-0 flex flex-col ">
          {data.selectedPlaces.map((place, index) =>
            place ? (
              <VisitPlaceCard key={index} visitPlaces={place.visitPlace} />
            ) : null
          )}
        </div>
      </div>
    </>
  );
};

export default TripDestinationComponent;
