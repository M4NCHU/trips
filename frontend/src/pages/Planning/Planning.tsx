import { GetVisitPlacesById } from "../../api/VisitPlaceAPI";
import { GetTripById } from "../../api/TripAPI";
import { Button } from "../../components/ui/button";
import { FC, useEffect, useState } from "react";
import { CiCircleInfo } from "react-icons/ci";
import { FaArrowLeft } from "react-icons/fa";
import { Link } from "react-router-dom";
import { GetDestinationById } from "../../api/Destinations";
import { Destination, DestinationCategory } from "../../types/Destination";
import { VisitPlace } from "../../types/VisitPlaceTypes";
import { TripDestination } from "../../types/TripDestinationTypes";
import { Trip } from "../../types/TripTypes";
import { fetchData } from "../../api/apiUtils";
import { IoMdClose } from "react-icons/io";
import { BsThreeDotsVertical } from "react-icons/bs";

interface PlanningProps {}

interface SelectedPlace {
  id: number;
  tripDestinationId: number;
  visitPlaceId: number;
  visitPlace: VisitPlace | null;
}

interface TripDestinationInterface {
  tripId: number;
  destinationId: number;
  destination: DestinationCategory;
  selectedPlaces: SelectedPlace[];
}

const Planning: FC<PlanningProps> = ({}) => {
  const [tripDestinations, setTripDestinations] = useState<
    TripDestinationInterface[]
  >([]);
  const [numberOfPeople, setNumberOfPeople] = useState<number>(1);

  const { data: trip, isLoading, isError } = GetTripById("1");

  useEffect(() => {
    const fetchVisitPlacesData = async () => {
      if (trip && trip.tripDestinations) {
        try {
          const tripDestinationsData = await Promise.all(
            trip.tripDestinations.map(async (tripDestination) => {
              // Fetch data for destination
              const destinationData = await fetchData<DestinationCategory>(
                `/api/Destinations/${tripDestination.destinationId}`
              );

              // Fetch data for each selectedPlace
              const selectedPlacesData = await Promise.all(
                tripDestination.selectedPlaces.map(async (selectedPlace) => {
                  const visitPlaceQuery = await fetchData<VisitPlace>(
                    `/api/VisitPlace/${selectedPlace.visitPlaceId}`
                  );
                  const visitPlaceData = visitPlaceQuery;

                  return {
                    ...selectedPlace,
                    visitPlace: visitPlaceData,
                    // You can add other properties as needed
                  };
                })
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

  console.log(tripDestinations);

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

  return (
    <div className="container px-4 mt-6 flex flex-col md:flex-row gap-4">
      <div className="w-full md:w-3/5 flex flex-col gap-4">
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
            <div
              className="bg-secondary p-4 rounded-xl flex flex-col gap-2 relative"
              key={i}
            >
              <button className="absolute top-4 right-4 text-2xl cursor-pointer p-2 rounded-xl hover:bg-pink-600">
                <IoMdClose />
              </button>

              <div className="flex flex-row items-center gap-4 mb-2">
                <div className="relative w-16 h-16">
                  <img
                    className="object-cover rounded-xl"
                    src={item.destination.photoUrl}
                    alt={item.destination.name}
                  />
                </div>

                <Link
                  to={`/destinations/details/${item.destination?.id}`}
                  className="text-xl font-semibold"
                >
                  {item.destination.name}
                </Link>
              </div>

              <hr />

              <div className="mt-2 px-0 md:px-4">
                {item.selectedPlaces.map((place, index) => (
                  <div
                    key={index}
                    className="border-l-2 px-4 py-2 rounded-xl rounded-l-none hover:bg-background flex flex-row items-center gap-4 justify-between"
                  >
                    <div className="flex flex-row items-center gap-4">
                      <div className="relative w-16 h-16">
                        <img
                          className="object-cover rounded-xl"
                          src={place.visitPlace?.photoUrl}
                          alt={place.visitPlace?.name}
                        />
                      </div>
                      <p className=" flex flex-col">
                        {place.visitPlace?.name}
                        <span className="block md:hidden text-sm">
                          {place.visitPlace?.price} zł /person
                        </span>
                      </p>
                    </div>
                    <div className="flex flex-row items-center gap-4">
                      <p className="hidden md:block">
                        {place.visitPlace?.price} zł
                        <span className="px-2">/</span>person
                      </p>
                      <button className="text-xl cursor-pointer p-2 rounded-xl hover:bg-pink-600">
                        <BsThreeDotsVertical />
                      </button>
                    </div>
                  </div>
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

          <div className="flex flex-col gap-2">
            <label htmlFor="persons-amount">How many people?</label>
            <input
              name="persons-amount"
              type="number"
              className="bg-background w-full rounded-lg p-2"
              placeholder="1"
              value={numberOfPeople}
              onChange={(e) => setNumberOfPeople(parseInt(e.target.value, 10))}
            />
          </div>

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
