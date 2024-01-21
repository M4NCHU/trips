import { FC, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { useDestinationById } from "../../api/Destinations";
import { UseCreateVisitPlace } from "../../api/VisitPlaceAPI";
import FormHeader from "../../components/Forms/FormHeader";
import Input from "../../components/Forms/Input";
import { Button } from "../../components/ui/button";
import useImagePreview from "src/hooks/useImagePreview";

interface CreateProps {}

interface FormValues {
  visitPlaceName: string;
  visitPlaceDesc: string;
  imageSrc: string;
  price: number;
  imageFile: File | null;
}

const initialFieldValues: FormValues = {
  visitPlaceName: "",
  visitPlaceDesc: "",
  imageSrc: "",
  price: 0,
  imageFile: null,
};

const CreateVisitPlace: FC<CreateProps> = ({}) => {
  const { id } = useParams<{ id: string | undefined }>();
  const [values, setValues] = useState<FormValues>(initialFieldValues);
  const { data: destination, isLoading, isError } = useDestinationById(id);
  const { showPreview, imagePreview } = useImagePreview();

  useEffect(() => {
    if (destination) {
      setValues({
        ...values,
        visitPlaceName: destination.name,
        visitPlaceDesc: destination.description,
        price: destination.price,
      });
    }
  }, [destination]);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setValues({
      ...values,
      [name]: value,
    });
  };

  const handleFormSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    // Sprawdź, czy destination jest dostępne
    if (!destination) {
      console.error("Error: No destination selected.");
      alert(
        "No destination selected. You cannot add a visit place without a destination."
      );
      return; // Przerwanie wykonania funkcji, jeśli brakuje destination
    }

    const formData = new FormData();
    formData.append("name", values.visitPlaceName);
    formData.append("description", values.visitPlaceDesc);
    formData.append("photoUrl", imagePreview.imageSrc);
    formData.append("price", (values.price ?? 2).toString());
    formData.append("destinationId", destination.id.toString());

    if (imagePreview.imageFile !== null) {
      formData.append("imageFile", imagePreview.imageFile);
    }

    try {
      await UseCreateVisitPlace(formData);
    } catch (error) {
      console.error("Error submitting form:", error);
      alert("An error occurred while submitting the form. Please try again.");
    }
  };

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (isError || !destination) {
    return <div>Error or no destination found.</div>;
  }

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
            <Link to={`/destination/${destination?.id}`} className="font-bold">
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
            <Input
              placeholder="Enter description"
              label="Price/person"
              name="price"
              value={values.price}
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
            {imagePreview.imageSrc && (
              <div className="img-preview">
                <p>Image Preview</p>
                <img src={imagePreview.imageSrc} alt="" />
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

export default CreateVisitPlace;
