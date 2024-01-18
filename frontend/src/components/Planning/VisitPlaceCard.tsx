import { FC } from "react";
import CardTitle from "./CardTitle";
import { VisitPlace } from "../../types/VisitPlaceTypes";
import { SelectedPlace } from "../../types/SelectedPlaceTypes";

interface VisitPlaceProps {
  data: SelectedPlace;
}

const VisitPlaceCard: FC<VisitPlaceProps> = ({ data }) => {
  return (
    <CardTitle
      image={data.visitPlace.photoUrl}
      alt={data.visitPlace.name}
      title={data.visitPlace.name}
    />
  );
};

export default VisitPlaceCard;
