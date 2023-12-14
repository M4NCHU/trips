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

interface FormValues {
  destinationName: string;
  destinationLocation: string;
  destinationDesc: string;
  imageSrc: string;
  imageFile: File | null;
}

const initialFieldValues: FormValues = {
  destinationName: "",
  destinationLocation: "",
  destinationDesc: "",
  imageSrc: "",
  imageFile: null,
};

const Create: FC<CreateProps> = ({}) => {
  // const [destinationName, setDestinationName] = useState<string>("");
  // const [destinationLocation, setDestinationLocation] = useState<string>("");
  // const [destinationDesc, setDestinationDesc] = useState<string>("");
  // const [imageSrc, setImageSrc] = useState<string>("");
  // const [imageFile, setImageFile] = useState<File | null>(null);

  const [values, setValues] = useState(initialFieldValues);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setValues({
      ...values,
      [name]: value,
    });
  };

  // console.log("imageFile", imageFile);
  // console.log("imageSrc", imageSrc);

  // const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
  //   if (e.target.files && e.target.files[0]) {
  //     const file = e.target.files[0];
  //     setImageFile(file);
  //     const reader = new FileReader();
  //     reader.onload = () => {
  //       if (typeof reader.result === "string") {
  //         setImageFile(file);
  //         setImageSrc(reader.result);
  //       }
  //     };
  //     reader.readAsDataURL(file);
  //   }
  // };

  const showPreview = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files[0]) {
      const imageFile: File = e.target.files[0];
      const reader = new FileReader();

      reader.onload = (x: ProgressEvent<FileReader>) => {
        if (x.target && typeof x.target.result === "string") {
          setValues({
            ...values,
            imageFile: imageFile,
            imageSrc: x.target.result,
          });
        }
      };

      reader.readAsDataURL(imageFile);
    } else {
      setValues({
        ...values,
        imageFile: null,
        imageSrc: "",
      });
    }
  };

  const handleFormSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const formData = new FormData();
    formData.append("name", values.destinationName);
    formData.append("location", values.destinationLocation);
    formData.append("description", values.destinationDesc);
    formData.append("photoUrl", values.imageSrc);

    if (values.imageFile !== null) {
      formData.append("imageFile", values.imageFile);
    }

    try {
      // Now you can proceed with the form submission using formData
      const response = await axios.post(
        "https://localhost:7154/api/Destinations",
        formData
      );

      console.log(response.data);
    } catch (error) {
      console.error("Error submitting form:", error);
    }
  };

  // const { mutate: createComment } = useMutation({
  //   mutationFn: async () => {
  //     const payload = {
  //       name: destinationName,
  //       location: destinationLocation,
  //       description: destinationDesc,
  //       photoUrl: imageSrc,
  //       imageFile: imageFile,
  //     };

  //     console.log("payload", payload);

  //     const { data } = await axios.post(
  //       "https://localhost:7154/api/Destinations",
  //       payload
  //     );

  //     console.log(data);
  //     return data;
  //   },
  // });

  return (
    <div className="container px-4">
      <div className="form-header flex flex-row py-6 gap-4 items-center">
        <Button className="bg-transparent text-foreground border-2 border-secondary hover:bg-secondary">
          <FaArrowLeft />
        </Button>
        <div className="flex flex-col justify-center">
          <span className="text-xs">Back to home page</span>
          <h1 className="text-2xl font-bold">Create new Destination</h1>
        </div>
      </div>
      <form
        className=" bg-secondary rounded-lg p-4 "
        onSubmit={handleFormSubmit}
      >
        <div className="flex flex-col md:flex-row gap-4">
          <div className="flex flex-col w-full gap-2">
            <h2 className="text-xl font-bold border-b-2 py-2 mb-4 ">
              General information
            </h2>
            <Input
              placeholder="Enter name"
              label="Name"
              name="destinationName"
              value={values.destinationName}
              onChange={handleInputChange}
            />
            <Input
              placeholder="Enter location"
              label="location"
              name="destinationLocation"
              value={values.destinationLocation}
              onChange={handleInputChange}
            />
            <Input
              placeholder="Enter description"
              label="description"
              name="destinationDesc"
              value={values.destinationDesc}
              onChange={handleInputChange}
            />
          </div>

          <div className="w-full md:w-1/3 flex flex-col">
            <h2 className="text-xl font-bold border-b-2 py-2 mb-4">
              Destination photo
            </h2>
            <Input
              placeholder="Choose photo"
              label="Choose photo"
              type="file"
              name="photoUrl"
              accept="image/*"
              onChange={showPreview}
              id="image-uploader"
            />
            {values.imageSrc && (
              <div className="img-preview">
                <p>Image Preview</p>
                <img src={values.imageSrc} alt="" />
              </div>
            )}
          </div>
        </div>
        <Button className="mt-4 w-full bg-red-400 " type="submit">
          Create +
        </Button>
      </form>
    </div>
  );
};

export default Create;
