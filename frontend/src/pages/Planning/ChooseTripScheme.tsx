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

  useEffect(() => {
    if (trips) {
      const filteredActiveTrips = trips.filter((trip) => trip.status === 1);
      setActiveTrips(filteredActiveTrips);
    }
  }, [trips]);

  return (
    <div className="container flex flex-col py-6 px-2 md:px-6">
      <PlanningHeader />
      <div className="flex flex-col gap-2">
        {activeTrips ? (
          <div>
            {activeTrips.map((trip, i) => (
              <TripCard key={i} data={trip} />
            ))}
          </div>
        ) : (
          "Nothing to see here"
        )}
      </div>
    </div>
  );
};

export default ChooseTripScheme;
