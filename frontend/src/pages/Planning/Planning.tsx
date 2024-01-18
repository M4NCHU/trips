import { FC, useEffect, useState } from "react";
import { BsThreeDotsVertical } from "react-icons/bs";
import { CiCircleInfo } from "react-icons/ci";
import { FaArrowLeft } from "react-icons/fa";
import { IoMdClose } from "react-icons/io";
import { Link } from "react-router-dom";
import { GetTripById } from "../../api/TripAPI";
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

interface PlanningProps {}

interface SelectedPlace {
  id: number;
  tripDestinationId: number;
  visitPlaceId: number;
  visitPlace: VisitPlace | null;
}

const Planning: FC<PlanningProps> = ({}) => {
  const [tripDestinations, setTripDestinations] = useState<
    TripDestinationInterface[]
  >([]);
  const [numberOfPeople, setNumberOfPeople] = useState<number>(1);
  const [participantsListData, setParticipantsListData] =
    useState<TripParticipant[]>();

  const { data: trip, isLoading, isError } = GetTripById("1");

  useEffect(() => {
    const fetchVisitPlacesData = async () => {
      if (trip && trip.tripDestinations) {
        try {
          const tripDestinationsData = await Promise.all(
            trip.tripDestinations.map(async (tripDestination) => {
              // Fetch data for destination
              const destinationData = await fetchData<DestinationCategory>(
                `/api/Destination/GetDestinationById/${tripDestination.destinationId}`
              );

              // Fetch data for each selectedPlaceParticipant
              const selectedPlacesData = await Promise.all(
                tripDestination.selectedPlaces.map(
                  async (selectedPlace: any) => {
                    const visitPlaceQuery = await fetchData<VisitPlace>(
                      `/api/VisitPlace/GetVisitPlaceById/${selectedPlace.visitPlaceId}`
                    );
                    const visitPlaceData = visitPlaceQuery;

                    return {
                      ...selectedPlace,
                      visitPlace: visitPlaceData,
                      // You can add other properties as needed
                    };
                  }
                )
              );

              return {
                ...tripDestination,
                destination: destinationData,
                selectedPlaces: selectedPlacesData,
                // You can add other properties as needed
              };
            })
          );

          // Update state with the fetched data
          setTripDestinations(tripDestinationsData);
        } catch (error) {
          console.error("Error fetching data:", error);
        }
      }
    };

    fetchVisitPlacesData();
  }, [trip]);

  const calculateTotalPrice = () => {
    let totalPrice = 0;

    tripDestinations.forEach((item) => {
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

  //     await Promise.all(

  //         const selectedPlacesData = await Promise.all(
  //           tripDestination.selectedPlaces.map(async (selectedPlace) => {
  //             if (selectedPlace.visitPlaceId) {
  //               const visitPlaceQuery = GetVisitPlacesById(
  //                 selectedPlace.visitPlaceId.toString()
  //               );
  //               const visitPlaceData = await visitPlaceQuery.data;
  //               return {
  //                 ...selectedPlace,
  //                 visitPlace: visitPlaceData,
  //               };
  //             } else {
  //               return selectedPlace;
  //             }
  //           })
  //         );
  //         return {
  //           ...tripDestination,
  //           destination: destinationData,
  //           selectedPlaces: selectedPlacesData,
  //         };
  //       })
  //     );
  //   setTripDestinations(tripDestinationsData);

  useEffect(() => {
    const fetchParticipantsList = async () => {
      if (trip && trip.id) {
        try {
          const ParticipantQuery = await fetchData<TripParticipant[]>(
            `/api/TripParticipant/GetTripParticipant/${trip.id}`
          );

          if (ParticipantQuery) {
            setParticipantsListData(ParticipantQuery);
          }
        } catch (error) {
          // Handle the error if necessary
          console.error("Error fetching participants:", error);
        }
      }
    };

    fetchParticipantsList();
  }, [trip]);

  console.log("participantsListData" + participantsListData);

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
          <div className="destination-header flex flex-row items-center gap-2">
            <h1 className="text-2xl font-bold">Chosen trip destinations</h1>
          </div>
          <hr />
          {tripDestinations.map((item, i) => (
            <div key={i} className="flex flex-col gap-2">
              <Card content={<DestinationCard data={item} />} />
              <div className="mt-2 px-0 pl-4 border-l-2 border-gray-500 flex flex-col">
                {item.selectedPlaces.map((place, index) => (
                  <Card key={index} content={<VisitPlaceCard data={place} />} />
                ))}
              </div>
            </div>
          ))}
          {/* {trip
            ? trip.destinations &&
              trip.destinations.map((item, i) => (
                <div key={i}>
                  <p>{item.name}</p>
                </div>
              ))
            : "no data"}
          {trip
            ? trip.visitPlaces &&
              trip.visitPlaces.map((item, i) => (
                <div key={i}>
                  <p>{item.name}</p>
                </div>
              ))
            : "no data"} */}
        </div>
        <div className="mt-4 flex flex-col gap-2">
          <div className="destination-header flex flex-row items-center gap-2 mb-4">
            <h1 className="text-2xl font-bold">Add trip participants</h1>
          </div>
          <hr />
          <div className="mt-2 flex flex-col gap-4">
            {participantsListData && participantsListData
              ? participantsListData.map((item, i) => (
                  <Card
                    key={i}
                    content={<ParticipantCard data={item.participants} />}
                  />
                ))
              : "No participants"}
          </div>
          <CreateParticipantModal />
        </div>
      </div>
      <div className="w-full md:w-2/5">
        <div className="reservation-card w-full bg-secondary p-6 rounded-xl flex flex-col gap-4">
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
