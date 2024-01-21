import { FC } from "react";
import CardTitle from "./CardTitle";
import { VisitPlace } from "../../types/VisitPlaceTypes";
import { SelectedPlace } from "../../types/SelectedPlaceTypes";
import {
  useVisitPlacesByDestination,
  useVisitPlacesById,
} from "src/api/VisitPlaceAPI";

interface VisitPlaceProps {
  data: string;
}

const VisitPlaceCard: FC<VisitPlaceProps> = ({ data }) => {
  const { data: visitPlaces, isLoading: isLoadingVisitPlaces } =
    useVisitPlacesById(data);

  if (!visitPlaces) {
    return (
      <div>
        <p>No data</p>
      </div>
    );
  }

  return (
    <CardTitle
      image={visitPlaces.photoUrl}
      alt={visitPlaces.name}
      title={visitPlaces.name}
    />
  );
};

export default VisitPlaceCard;
