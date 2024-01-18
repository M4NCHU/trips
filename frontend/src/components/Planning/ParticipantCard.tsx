import { FC } from "react";
import { Participant } from "../../types/ParticipantTypes";
import CardTitle from "./CardTitle";

interface ParticipantCardProps {
  data: Participant;
}

const ParticipantCard: FC<ParticipantCardProps> = ({ data }) => {
  return (
    <CardTitle
      image={data.photoUrl}
      alt={data.lastName}
      title={data.firstName}
    />
  );
};

export default ParticipantCard;
