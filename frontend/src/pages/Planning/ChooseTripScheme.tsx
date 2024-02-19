import { FC, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import PlanningHeader from "../../components/Planning/ChoosenTrip/PlanningHeader";
import TripCard from "../../components/Planning/ChoosenTrip/TripCard";
import { UseUserTripsList } from "src/api/TripAPI";
import { useAuth } from "src/context/UserContext";
import { Trip } from "src/types/TripTypes";
import NoData from "src/components/Error/404/NoData";
import DefaultLoading from "src/components/Loading/DefaultLoading";

interface ChooseTripSchemeProps {}

const ChooseTripScheme: FC<ChooseTripSchemeProps> = ({}) => {
  const { user } = useAuth();
  const [activeTrips, setActiveTrips] = useState<Trip[]>([]);

  const { data: trips, isLoading } = UseUserTripsList(user?.id);

  if (isLoading) {
    return <DefaultLoading />;
  }

  const planning = trips?.filter((t) => t.status === 0);
  const inProgress = trips?.filter((t) => t.status === 1);
  const finished = trips?.filter((t) => t.status === 2);

  return (
    <div className="container flex flex-col py-6 px-2 md:px-6">
      {trips ? (
        <>
          <PlanningHeader />
          <div className="flex flex-col gap-2">
            {planning ? (
              <div>
                {planning.map((trip, i) => (
                  <TripCard key={i} data={trip} />
                ))}
              </div>
            ) : (
              ""
            )}

            {inProgress ? (
              <div>
                {inProgress.map((trip, i) => (
                  <TripCard key={i} data={trip} />
                ))}
              </div>
            ) : (
              <p></p>
            )}

            {finished ? (
              <div>
                {finished.map((trip, i) => (
                  <TripCard key={i} data={trip} />
                ))}
              </div>
            ) : (
              <p></p>
            )}
          </div>
        </>
      ) : (
        <NoData />
      )}
    </div>
  );
};

export default ChooseTripScheme;
