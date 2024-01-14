import { GetDestinationById } from "../../api/Destinations";
import { FC } from "react";
import { redirect, useNavigate, useParams } from "react-router-dom";
import { Button } from "../ui/button";
import { FaRegHeart } from "react-icons/fa";
import { CiStar } from "react-icons/ci";
import { CiCirclePlus } from "react-icons/ci";
import { CiCircleInfo } from "react-icons/ci";

interface DestinationDetailsProps {}

const DestinationDetails: FC<DestinationDetailsProps> = ({}) => {
  const { id } = useParams();

  if (id) {
    const { data: destination, isLoading, isError } = GetDestinationById(id);

    return destination ? (
      <div className="destination-details container my-6 flex flex-col px-4 md:px-16 gap-6">
        <div className="destination-header">
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
          </div>
          <div className="w-full md:w-2/5 flex">
            <div className="reservation-card w-full bg-secondary p-6 rounded-xl flex flex-col gap-4 grow">
              <div>
                <p className="text-sm">Initial Charge on this trip is 24 zł</p>
              </div>
              <div className="destination-price flex flex-row items-center gap-1">
                <h4 className="text-xl font-bold">
                  <span className="font-normal">Total:</span> 24 zł{" "}
                  <span className="font-normal"> / person</span>
                </h4>
                <CiCircleInfo className="text-xl cursor-pointer" />
              </div>

              <Button className="bg-pink-700 text-white font-semibold text-lg p-6">
                Add to your trip plan
              </Button>

              <div className="flex justify-center">
                <p className="">You won't be charged yet</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    ) : (
      <p>no data</p>
    );
  }
  return null;
};

export default DestinationDetails;
