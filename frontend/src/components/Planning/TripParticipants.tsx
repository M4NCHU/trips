import { FC, useState } from "react";
import ParticipantCard from "./ParticipantCard";
import CreateParticipantModal from "../TripParticipants/CreateParticipantModal";
import { UseTripParticipant } from "src/api/TripParticipantAPI";
import Card from "./Card";
import PlanningHeader from "./PlanningHeader";

interface TripParticipantsProps {
  tripId: string;
}

const TripParticipants: FC<TripParticipantsProps> = ({ tripId }) => {
  const [participants, setParticipants] = useState([]);
  const {
    data: TripParticipantsData,
    isLoading,
    isError,
    refetch,
  } = UseTripParticipant(tripId);

  const onParticipantAdded = () => {
    refetch();
  };

  if (isLoading) {
    return (
      <div>
        <p>Loading data</p>
      </div>
    );
  }

  if (isError) {
    return (
      <div>
        <p>There is an error</p>
      </div>
    );
  }

  return (
    <div className="mt-4 flex flex-col gap-2">
      <PlanningHeader title="Chosen trip destinations" />
      <hr />
      <div className="mt-2 flex flex-col gap-4">
        {TripParticipantsData
          ? TripParticipantsData.map((item, i) => (
              <Card
                key={i}
                content={<ParticipantCard data={item.participants} />}
              />
            ))
          : "No participants"}
      </div>
      <CreateParticipantModal
        tripId={tripId}
        onParticipantAdded={onParticipantAdded}
      />
    </div>
  );
};

export default TripParticipants;
