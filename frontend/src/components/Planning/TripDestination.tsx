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
  const { data: destinationData, isLoading: isLoadingDestination } =
    useDestinationById(data.destinationId);

  if (isLoadingDestination) {
    return <div>Loading destination details...</div>;
  }

  if (!destinationData) {
    return <div>Failed to load destination or visit places</div>;
  }

  return (
    <div className="flex flex-col gap-2">
      <Card content={<DestinationCard data={destinationData} />} />
      <div className="mt-2 px-0 pl-4 border-l-2 border-gray-500 flex flex-col">
        {data.selectedPlaces.map((place, index) =>
          place.visitPlaceId ? (
            <Card
              key={index}
              content={<VisitPlaceCard key={index} data={place.visitPlaceId} />}
            />
          ) : null
        )}
      </div>
    </div>
  );
};

export default TripDestinationComponent;
