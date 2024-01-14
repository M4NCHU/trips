import { FC, useState } from "react";
import { createVisitPlace } from "../../api/VisitPlaceAPI";
import FormHeader from "../../components/Forms/FormHeader";
import Input from "../../components/Forms/Input";
import { Button } from "../../components/ui/button";
import { GetDestinationById } from "../../api/Destinations";
import { Link, Navigate, redirect, useParams } from "react-router-dom";

interface CreateProps {}

interface FormValues {
  visitPlaceName: string;
  visitPlaceDesc: string;
  imageSrc: string;
  imageFile: File | null;
}

const initialFieldValues: FormValues = {
  visitPlaceName: "",
  visitPlaceDesc: "",
  imageSrc: "",
  imageFile: null,
};

const CreateVisitPlace: FC<CreateProps> = ({}) => {
  const { id } = useParams();
  const [values, setValues] = useState(initialFieldValues);

  if (id) {
    const { data: destination, isLoading, isError } = GetDestinationById(id);

    if (destination) {
      const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setValues({
          ...values,
          [name]: value,
        });
      };

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
        formData.append("name", values.visitPlaceName);
        formData.append("description", values.visitPlaceDesc);
        formData.append("photoUrl", values.imageSrc);
        formData.append("destinationId", destination.id.toString());

        if (values.imageFile !== null) {
          formData.append("imageFile", values.imageFile);
        }

        try {
          await createVisitPlace(formData);
        } catch (error) {
          console.error("Error submitting form:", error);
        }
      };

      return (
        <div className="container px-4">
          <FormHeader title="Create new visit place" />
          <form
            className=" bg-secondary rounded-lg p-4 "
            onSubmit={handleFormSubmit}
          >
            <div className="bg-background p-4 mb-4 rounded-lg flex flex-row justify-between">
              <div className="flex flex-row items-center gap-2">
                <p>Creating visit place on: </p>
                <Link
                  to={`/destinations/details/${destination?.id}`}
                  className="font-bold"
                >
                  {destination?.name}
                </Link>
              </div>
              <button>Change</button>
            </div>
            <div className="flex flex-col md:flex-row gap-4">
              <div className="flex flex-col w-full gap-2">
                <h2 className="text-xl font-bold border-b-2 py-2 mb-4 ">
                  General information
                </h2>
                <Input
                  placeholder="Enter name"
                  label="Name"
                  name="visitPlaceName"
                  value={values.visitPlaceName}
                  onChange={handleInputChange}
                />
                <Input
                  placeholder="Enter description"
                  label="description"
                  name="visitPlaceDesc"
                  value={values.visitPlaceDesc}
                  onChange={handleInputChange}
                />
              </div>

              <div className="w-full md:w-1/3 flex flex-col">
                <h2 className="text-xl font-bold border-b-2 py-2 mb-4">
                  Visit place photo
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
    }
  }
  return null;
};

export default CreateVisitPlace;
