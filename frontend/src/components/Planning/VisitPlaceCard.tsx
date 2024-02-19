import { FC } from "react";
import CardTitle from "./CardTitle";
import { VisitPlace } from "../../types/VisitPlaceTypes";
import { SelectedPlace } from "../../types/SelectedPlaceTypes";
import {
  useVisitPlacesByDestination,
  useVisitPlacesById,
} from "src/api/VisitPlaceAPI";
import { BsDot } from "react-icons/bs";

interface VisitPlaceProps {
  visitPlaces: VisitPlace;
}

const VisitPlaceCard: FC<VisitPlaceProps> = ({ visitPlaces }) => {
  // const { data: visitPlaces, isLoading: isLoadingVisitPlaces } =
  //   useVisitPlacesById(data);

  if (!visitPlaces) {
    return (
      <div>
        <p>No data</p>
      </div>
    );
  }

  return (
    <>
      <div className="flex flex-row gap-2">
        <div className="flex flex-col bg-secondary grow min-h-[5rem] rounded-lg p-2 gap-2">
          <div className="flex gap-2 flex-col">
            <h2 className="text-xl font-semibold">{visitPlaces.name}</h2>
            <p>{visitPlaces.description}</p>
          </div>
          <div className="w-auto flex flex-row items-center">
            <p className="bg-background py-1 px-2 rounded-full">
              7:30 AM - 7:30 AM
            </p>
            <span>
              <BsDot />
            </span>
            <p className="font-bold text-lg">
              {visitPlaces.price} zł{" "}
              <span className="font-normal text-gray-500">/ person</span>
            </p>
          </div>

          {/* <CardTitle
        image={data.photoUrl}
        alt={data.name}
        title={data.name}
        link={`/destination/${data.id}`}
      /> */}
        </div>
        <div className="w-[5rem] h-[5rem] relative">
          <img
            src={visitPlaces.photoUrl}
            alt={visitPlaces.name}
            className="rounded-lg object-cover"
          />
        </div>
      </div>
      {visitPlaces && (
        <div className="ml-6 p-2 border-l-2 border-gray-500">
          <h1 className="px-2">12h</h1>
        </div>
      )}
    </>
  );
};

export default VisitPlaceCard;
