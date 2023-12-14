import TextArea from "../../components/Forms/TextArea";
import Input from "../../components/Forms/Input";
import { Button } from "../../components/ui/button";
import { Destinations } from "@/src/types/Destinations";
import { useMutation, useQuery } from "@tanstack/react-query";
import axios from "axios";
import { FC, useState } from "react";
import { FaArrowLeft } from "react-icons/fa6";
import { DestinationPayload } from "../../lib/validators/destination";

interface CreateProps {}

const Create: FC<CreateProps> = ({}) => {
  const [destinationName, setDestinationName] = useState<string>("");
  const [destinationLocation, setDestinationLocation] = useState<string>("");
  const [destinationDesc, setDestinationDesc] = useState<string>("");

  const destinations = useQuery({
    queryKey: ["destinations"],
    queryFn: async () => {
      const { data } = await axios.get(
        "https://localhost:7154/api/Destinations"
      );

      return data as Destinations[];
    },
  });

  const { mutate: createComment } = useMutation({
    mutationFn: async () => {
      const payload: DestinationPayload = {
        name: destinationName,
        location: destinationLocation,
        description: destinationDesc,
      };

      console.log("payload", payload);

      const { data } = await axios.post(
        "https://localhost:7154/api/Destinations",
        payload
      );

      console.log(data);
      return data;
    },
  });

  return (
    <div className="container">
      <div className="form-header flex flex-row py-6 gap-4 items-center">
        <Button className="bg-transparent text-foreground border-2 border-secondary hover:bg-secondary">
          <FaArrowLeft />
        </Button>
        <div className="flex flex-col justify-center">
          <span className="text-xs">Back to home page</span>
          <h1 className="text-2xl font-bold">Create new Destination</h1>
        </div>
      </div>
      <div className=" bg-secondary rounded-lg p-4 ">
        <div className="flex flex-row gap-4">
          <div className="flex flex-col w-full gap-2">
            <h2 className="text-xl font-bold border-b-2 py-2 mb-4 ">
              General information
            </h2>
            <Input
              placeholder="Enter name"
              label="Name"
              name="name"
              value={destinationName}
              onChange={(e) => setDestinationName(e.target.value)}
            />
            <Input
              placeholder="Enter location"
              label="location"
              name="location"
              value={destinationLocation}
              onChange={(e) => setDestinationLocation(e.target.value)}
            />
            <TextArea
              placeholder="Enter description"
              label="description"
              name="description"
              value={destinationDesc}
              onChange={(e) => setDestinationDesc(e.target.value)}
            />
          </div>

          <div className="w-1/3">
            <h2 className="text-xl font-bold border-b-2 py-2 mb-4">
              Destination photo
            </h2>
            <Input
              placeholder="Choose photo"
              label="Choose photo"
              type="file"
              name="photoUrl"
            />
          </div>
        </div>
        <Button
          className="mt-4 w-full bg-red-400 "
          onClick={() => createComment()}
        >
          Create +
        </Button>
      </div>

      {destinations.data?.map((item, i) => (
        <p key={i}>{item.name}</p>
      ))}
    </div>
  );
};

export default Create;
