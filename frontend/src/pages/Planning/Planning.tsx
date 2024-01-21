import { FC, useEffect, useState } from "react";
import { BsThreeDotsVertical } from "react-icons/bs";
import { CiCircleInfo } from "react-icons/ci";
import { FaArrowLeft } from "react-icons/fa";
import { IoMdClose } from "react-icons/io";
import { Link, useParams } from "react-router-dom";
import { UseTripById } from "../../api/TripAPI";
import { fetchData } from "../../api/apiUtils";

import CreateParticipantModal from "../../components/TripParticipants/CreateParticipantModal";
import { Button } from "../../components/ui/button";
import { DestinationCategory } from "../../types/Destination";
import { TripParticipant } from "../../types/TripParticipantTypes";
import { VisitPlace } from "../../types/VisitPlaceTypes";
import Card from "../../components/Planning/Card";
import ParticipantCard from "../../components/Planning/ParticipantCard";
import DestinationCard from "../../components/Planning/DestinationCard";
import { TripDestinationInterface } from "../../types/TripDestinationTypes";
import VisitPlaceCard from "../../components/Planning/VisitPlaceCard";
import { useDestinationById } from "src/api/Destinations";
import { useVisitPlacesByDestination } from "src/api/VisitPlaceAPI";
import TripDestinationComponent from "src/components/Planning/TripDestination";
import TripParticipants from "src/components/Planning/TripParticipants";
import PlanningHeader from "src/components/Planning/PlanningHeader";

interface PlanningProps {}

interface SelectedPlace {
  id: string;
  tripDestinationId: string;
  visitPlaceId: string;
  visitPlace: VisitPlace | null;
}

const Planning: FC<PlanningProps> = ({}) => {
  const { id } = useParams<{ id: string | undefined }>();
  const [numberOfPeople, setNumberOfPeople] = useState<number>(1);
  const [participantsListData, setParticipantsListData] =
    useState<TripParticipant[]>();

  const {
    data: trip,
    isLoading: isLoadingTrip,
    isError: isErrorTrip,
  } = UseTripById(id);

  if (isLoadingTrip) {
    return <div>Loading...</div>;
  }

  if (isErrorTrip || !trip) {
    return <div>Error or no trip data.</div>;
  }

  const calculateTotalPrice = () => {
    let totalPrice = 0;

    trip.tripDestinations.forEach((item) => {
      totalPrice += item.destination?.price || 0;

      item.selectedPlaces.forEach((place) => {
        totalPrice += place.visitPlace?.price || 0;
      });
    });

    // Multiply by the number of people
    totalPrice *= numberOfPeople;

    // Format to two decimal places
    return totalPrice.toFixed(2);
  };

  return (
    <div className="container px-4 mt-6 flex flex-col md:flex-row gap-4">
      <div className="w-full md:w-3/5 flex flex-col gap-6">
        <div className="destination-header flex flex-row items-center gap-2 mb-4">
          <Link to={`/`} className="flex flex-row items-center gap-2">
            <FaArrowLeft />
            Back to home{" "}
          </Link>
          <span>/</span>
          <h1 className="text-2xl font-bold">Plan your trip</h1>
        </div>
        <div className="flex flex-col gap-4">
          <PlanningHeader title="Chosen trip destinations" />
          <hr />
          {trip.tripDestinations.map((tripDestination, index) => (
            <TripDestinationComponent key={index} data={tripDestination} />
          ))}
        </div>
        <TripParticipants tripId={trip.id} />
      </div>
      <div className="w-full md:w-2/5 ">
        <div className="reservation-card w-full bg-secondary p-6 rounded-xl flex flex-col gap-4 sticky top-10">
          <div className="destination-price flex flex-row items-center gap-1">
            <h4 className="text-xl font-bold">
              <span className="font-normal">Total:</span>{" "}
              {calculateTotalPrice()} zł
            </h4>
            <CiCircleInfo className="text-xl cursor-pointer" />
          </div>

          <div className="flex flex-col gap-2"></div>

          <Button className="bg-pink-700 text-white font-semibold text-lg p-6">
            Reserve
          </Button>

          <div className="flex justify-center">
            <p className="">You won't be charged yet</p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Planning;
