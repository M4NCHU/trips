import { FC, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import PlanningHeader from "./PlanningHeader";
import TripCard from "./TripCard";
import { UseUserTripsList } from "src/api/TripAPI";
import { useAuth } from "src/context/UserContext";
import { Trip } from "src/types/TripTypes";

interface ChooseTripSchemeProps {}

const ChooseTripScheme: FC<ChooseTripSchemeProps> = ({}) => {
  const { user } = useAuth();
  const [activeTrips, setActiveTrips] = useState<Trip[]>([]);

  const { data: trips } = UseUserTripsList(user?.id);

  const planning = trips?.filter((t) => t.status === 0);
  const inProgress = trips?.filter((t) => t.status === 1);
  const finished = trips?.filter((t) => t.status === 2);

  return (
    <div className="container flex flex-col py-6 px-2 md:px-6">
      <PlanningHeader />
      <div className="flex flex-col gap-2">
        {planning ? (
          <div>
            {planning.map((trip, i) => (
              <TripCard key={i} data={trip} />
            ))}
          </div>
        ) : (
          "Nothing to see here"
        )}

        {inProgress ? (
          <div>
            {inProgress.map((trip, i) => (
              <TripCard key={i} data={trip} />
            ))}
          </div>
        ) : (
          <p>Nothing to see here</p>
        )}

        {finished ? (
          <div>
            {finished.map((trip, i) => (
              <TripCard key={i} data={trip} />
            ))}
          </div>
        ) : (
          <p>Nothing to see here</p>
        )}
      </div>
    </div>
  );
};

export default ChooseTripScheme;
