import { FC, useEffect, useState } from "react";
import { CiCircleInfo } from "react-icons/ci";
import { FaArrowLeft } from "react-icons/fa";
import { Link, useNavigate, useParams } from "react-router-dom";
import { UseChangeTripTitle, UseTripById } from "../../api/TripAPI";

import PlanningHeader from "src/components/Planning/PlanningHeader";
import TripDestinationComponent from "src/components/Planning/TripDestination";
import TripParticipants from "src/components/Planning/TripParticipants";
import { Button } from "../../components/ui/button";
import { TripParticipant } from "../../types/TripParticipantTypes";
import { VisitPlace } from "../../types/VisitPlaceTypes";
import { MdClose } from "react-icons/md";
import { GoPencil } from "react-icons/go";

interface PlanningProps {}

interface SelectedPlace {
  id: string;
  tripDestinationId: string;
  visitPlaceId: string;
  visitPlace: VisitPlace | null;
}

const Planning: FC<PlanningProps> = ({}) => {
  const { id } = useParams<{ id: string | undefined }>();
  const navigate = useNavigate();
  const {
    data: trip,
    isLoading: isLoadingTrip,
    isError: isErrorTrip,
  } = UseTripById(id);

  const [numberOfPeople, setNumberOfPeople] = useState<number>(1);
  const [participantsListData, setParticipantsListData] =
    useState<TripParticipant[]>();
  const [isTitleEditable, setIsTitleEditable] = useState(false);
  const [editableTitle, setEditableTitle] = useState(trip ? trip?.title : "");

  useEffect(() => {
    // Redirect to home page if id is not available
    if (!id) {
      navigate("/");
    }
  }, [id]);

  const handleTitleClick = () => {
    setIsTitleEditable(true);
  };

  const handleTitleChange = (e: any) => {
    setEditableTitle(e.target.value);
  };

  const handleSaveClick = async (e: any) => {
    e.preventDefault();
    console.log(id, editableTitle);
    try {
      if (id) {
        const ChangeTitleResult = await UseChangeTripTitle(id, editableTitle);
        if (ChangeTitleResult) {
          setIsTitleEditable(false);
        }
      }
    } catch (error) {
      console.error("Error submitting form:", error);
    }
  };

  const handleCancelClick = () => {
    if (trip) {
      setEditableTitle(trip.title);
    }
    setIsTitleEditable(false);
  };

  useEffect(() => {
    if (trip) {
      setEditableTitle(trip.title);
    }
  }, [trip]);

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
        <div className="destination-header flex flex-row items-center gap-4 mb-4">
          <Link to={`/`} className="flex flex-row items-center gap-2">
            <FaArrowLeft />
            <p className="hidden md:block">Back to home</p>
          </Link>
          <span>/</span>
          <Link
            to={`/planning`}
            className="flex flex-row items-center gap-2 text-sm md:text-base"
          >
            Trip schemes
          </Link>
          <span>/</span>
          <h1 className="text-xl md:text-2xl font-bold">Plan your trip</h1>
        </div>
        <div className="w-full bg-secondary p-4 flex items-center gap-8 justify-between rounded-lg">
          <div className="grow">
            {isTitleEditable ? (
              <div className="flex flex-row gap-2">
                <input
                  type="text"
                  value={editableTitle}
                  onChange={handleTitleChange}
                  className="white rounded-lg bg-background outline-none w-full h-full  p-2"
                  autoFocus
                />

                <button
                  onClick={handleCancelClick}
                  className=" bg-red-500 rounded-md p-2"
                >
                  <MdClose />
                </button>
                <button
                  onClick={(e) => handleSaveClick(e)}
                  className=" bg-green-500 rounded-md p-2"
                >
                  <GoPencil />
                </button>
              </div>
            ) : (
              <div
                className="flex flex-row items-center gap-1 p-2 cursor-pointer hover:bg-background rounded-lg "
                onClick={handleTitleClick}
              >
                <p className="bg-transparent m-0">{editableTitle}</p>
              </div>
            )}
          </div>

          <div className="flex items-center gap-2">
            <span className="text-gray-500">Created by:</span>
            <p className="font-semibold">{trip.user?.firstName}</p>
          </div>
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
