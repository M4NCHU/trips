import { FC, useEffect, useState } from "react";
import {
  CiCircleInfo,
  CiCirclePlus,
  CiSquarePlus,
  CiStar,
} from "react-icons/ci";
import { FaArrowLeft, FaRegHeart } from "react-icons/fa";
import { Link, useParams } from "react-router-dom";
import { useDestinationById } from "../../api/Destinations";
import { useVisitPlacesByDestination } from "../../api/VisitPlaceAPI";
import { Button } from "../ui/button";
import { UseCreateTripDestination } from "../../api/TripDestinationAPI";
import { SelectedPlace } from "src/types/SelectedPlaceTypes";
import { useAuth } from "src/context/UserContext";
import { UseUserTripsList } from "src/api/TripAPI";
import { User } from "src/types/UserTypes";
import { Trip } from "src/types/TripTypes";

interface DestinationDetailsProps {}

const DestinationDetails: FC<DestinationDetailsProps> = ({}) => {
  const { user } = useAuth();

  const { data: tripsData } = UseUserTripsList(user?.id);

  console.log(tripsData);
  const { id } = useParams<{ id: string | undefined }>();
  const [selectedPlaces, setSelectedPlaces] = useState<
    { id: string; price: number }[]
  >([]);
  const [activeTrips, setActiveTrips] = useState<Trip[]>([]);
  const [totalPrice, setTotalPrice] = useState(24);
  const [tripDestinationId, setTripDestinationId] = useState<string>("");

  const {
    data: destination,
    isLoading: isLoadingDestination,
    isError: isErrorDestination,
  } = useDestinationById(id);
  const {
    data: visitPlaces,
    isLoading: isLoadingVisitPlaces,
    isError: isErrorVisitPlaces,
  } = useVisitPlacesByDestination(id);

  useEffect(() => {
    if (tripsData) {
      const filteredActiveTrips = tripsData.filter((trip) => trip.status === 1);
      setActiveTrips(filteredActiveTrips);
    }
  }, [tripsData]);

  useEffect(() => {
    if (!id) {
      setSelectedPlaces([]);
      setTotalPrice(24);
    }

    if (tripsData && tripsData.length > 0 && !tripDestinationId) {
      setTripDestinationId(tripsData[0].id);
    }
  }, [id, tripsData, tripDestinationId]);

  const handleAddToTrip = (placeId: string, placePrice: number) => {
    // Sprawdź, czy element o danym id już istnieje na liście
    const existingPlaceIndex = selectedPlaces.findIndex(
      (selectedPlace) => selectedPlace.id === placeId
    );

    if (existingPlaceIndex !== -1) {
      // Jeżeli istnieje, usuń element z listy i zaktualizuj łączną cenę
      setSelectedPlaces((prevSelectedPlaces) => {
        const updatedSelectedPlaces = [...prevSelectedPlaces];
        const removedPlace = updatedSelectedPlaces.splice(
          existingPlaceIndex,
          1
        )[0];
        const updatedTotalPrice = totalPrice - removedPlace.price;
        setTotalPrice(updatedTotalPrice);
        return updatedSelectedPlaces;
      });
    } else {
      // Jeżeli nie istnieje, dodaj nowy element do listy i zaktualizuj łączną cenę
      setSelectedPlaces((prevSelectedPlaces) => [
        ...prevSelectedPlaces,
        { id: placeId, price: placePrice },
      ]);

      setTotalPrice((prevTotalPrice) => prevTotalPrice + placePrice);
    }
  };

  if (isLoadingDestination || isLoadingVisitPlaces) {
    return <div>Loading...</div>;
  }

  if (isErrorDestination || isErrorVisitPlaces || !destination) {
    return <div>Error or no destination found.</div>;
  }

  const handleFormSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!id) {
      console.error("Error: No destination ID provided");
      return;
    }

    if (!tripsData) {
      console.error("Error: No destination ID provided");
      return;
    }
    const formData = new FormData();
    formData.append(
      "TripId",
      tripDestinationId ? tripDestinationId : tripsData[0].id
    );
    formData.append("DestinationId", id);
    selectedPlaces.forEach((place, index) => {
      formData.append(`selectedPlaces[${index}].visitPlaceId`, place.id);
    });

    try {
      await UseCreateTripDestination(formData);
    } catch (error) {
      console.error("Error submitting form:", error);
    }
  };

  return (
    <div className="destination-details container my-6 flex flex-col px-4 md:px-16 gap-6">
      <div className="destination-header flex flex-row items-center gap-2">
        <Link to={`/`} className="flex flex-row items-center gap-2">
          <FaArrowLeft />
          Back to home{" "}
        </Link>
        <span>/</span>
        <h1 className="text-2xl font-bold">{destination.name}</h1>
      </div>

      <section className="destination-reservation flex flex-row gap-4 justify-center">
        <div className="relative w-full ">
          <img
            src={destination.photoUrl}
            alt={`${destination.name}`}
            className="w-full  object-cover rounded-xl h-[28rem]"
          />
        </div>
      </section>
      <div className="flex flex-col md:flex-row justify-center gap-6">
        <div className="w-full md:w-3/5 flex flex-col gap-6">
          <div className="flex flex-row justify-between">
            <div className="flex flex-row gap-4 items-center">
              <h2 className="text-2xl font-bold">{destination.name}</h2>
              <div className="w-2 h-[1px] bg-gray-500"></div>
              <p className="text-lg font-normal text-gray-500">
                {destination.category.name}
              </p>
            </div>

            <FaRegHeart className="text-2xl font-bold cursor-pointer hover:text-pink-700" />
          </div>
          <div className="w-full rounded-xl py-4 border-[1px] border-secondary flex flex-row justify-between items-center px-6">
            <div className="flex flex-row">
              <div className="flex justify-center flex-col items-center gap-2 border-r-[1px] pr-4">
                5.0
                <div className="flex flex-row">
                  <CiStar />
                  <CiStar />
                  <CiStar />
                  <CiStar />
                  <CiStar />
                </div>
              </div>
              <div className="flex justify-center flex-col items-center gap-2 border-r-[1px] px-6">
                <p>12</p>
                <p className="font-light">Reviews</p>
              </div>
            </div>
            <div className="">
              <button className="flex flex-row gap-2 hover:text-gray-400 p-2">
                <p>Create your own review</p>
                <CiCirclePlus className="text-2xl font-bold" />
              </button>
            </div>
          </div>

          <hr />

          <div>
            <p>{destination.description}</p>
          </div>

          <hr />

          <div className="destination-visit__places">
            <div className=" flex justify-between">
              <h4 className="text-xl font font-semibold">
                Choose which places you want to visit
              </h4>
              <Link to={`/destination/${destination.id}/visit-place/create`}>
                <CiCirclePlus className="text-2xl font font-semibold" />
              </Link>
            </div>

            <div className="flex flex-col gap-4 mt-4">
              {visitPlaces
                ? visitPlaces.map((place, i) => (
                    <div
                      className="flex flex-row justify-between items-center w-full bg-secondary p-2 rounded-xl"
                      key={i}
                    >
                      <div className="flex flex-row items-center gap-4">
                        <div className="relative flex items-center justify-center  w-[6rem] h-[6rem]">
                          <img
                            src={place.photoUrl}
                            alt={place.name}
                            className="object-cover rounded-xl"
                          />
                        </div>
                        <p className="font-semibold">{place.name}</p>
                      </div>
                      <div className="flex flex-row gap-4 px-4 items-center">
                        <p>{place.price} zł / person</p>
                        <button
                          className=""
                          onClick={() =>
                            handleAddToTrip(String(place.id), place.price)
                          }
                        >
                          <CiSquarePlus
                            className={`text-5xl font-bold  ${
                              selectedPlaces.some(
                                (selected) => selected.id === place.id
                              )
                                ? "bg-pink-500 hover:bg-pink-600" // Różowy kolor, jeżeli element jest na liście
                                : "hover:text-pink-600"
                            } rounded-xl`}
                          />
                        </button>
                      </div>
                    </div>
                  ))
                : "No places to visit"}
            </div>
          </div>
        </div>
        <div className="w-full md:w-2/5">
          <div className="reservation-card w-full bg-secondary p-6 rounded-xl flex flex-col gap-4 sticky top-10">
            <div>
              <p className="text-sm">Initial Charge on this trip is 24 zł</p>
            </div>
            <div className="destination-price flex flex-row items-center gap-1">
              <h4 className="text-xl font-bold">
                <span className="font-normal">Total:</span>{" "}
                {totalPrice.toFixed(2)} zł{" "}
                <span className="font-normal"> / person</span>
              </h4>
              <CiCircleInfo className="text-xl cursor-pointer" />
            </div>

            {activeTrips && activeTrips.length > 1 ? (
              <select
                className="bg-pink-700 text-white font-semibold text-lg p-4 rounded-lg"
                value={tripDestinationId}
                onChange={(e) => setTripDestinationId(e.target.value)}
              >
                {activeTrips.map((trip, i) => (
                  <option className="bg-pink-700" key={i} value={trip.id}>
                    {trip.title}
                  </option>
                ))}
              </select>
            ) : (
              <Button
                className="bg-pink-700 text-white font-semibold text-lg p-6"
                onClick={handleFormSubmit}
              >
                {" "}
                Add to your trip plan
              </Button>
            )}

            <div className="flex justify-center">
              <p className="">You won't be charged yet</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default DestinationDetails;
