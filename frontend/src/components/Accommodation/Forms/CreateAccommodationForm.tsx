import { FC, useState } from "react";
import toast from "react-hot-toast";
import { useNavigate } from "react-router-dom";
import SubmitButton from "src/components/ui/SubmitButton";
import useForm from "src/hooks/useForm";
import useImagePreview from "src/hooks/useImagePreview";
import FormHeader from "src/components/Forms/FormHeader";
import Input from "src/components/Forms/Input";
import MapModal from "src/components/Maps/Picker/MapPicker";
import { GeoLocation } from "src/types/GeoLocation/GeoLocation";
import { LatLngTuple } from "leaflet";
import { UseCreateAccommodation } from "src/api/AccommodationsAPI";
import { AccommodationValidator } from "src/lib/validators/AccommodationValidator";

interface CreateProps {}

interface FormValues {
  name: string;
  description: string;
  location: string;
  price: string;
  bedAmount: string;
}

const initialFieldValues: FormValues = {
  name: "",
  description: "",
  location: "",
  price: "",
  bedAmount: "",
};

const CreateAccommodationForm: FC<CreateProps> = ({}) => {
  const {
    values,
    errors,
    handleChange,
    validate,
    getFormData,
    setValue,
    reset,
  } = useForm(initialFieldValues, AccommodationValidator);
  const { showPreview, imagePreview } = useImagePreview();

  const [geoLocation, setGeoLocation] = useState<GeoLocation | null>(null);

  const {
    mutate: CreateAccommodation,
    isPending: isCreating,
    isSuccess: isSuccessCreating,
  } = UseCreateAccommodation();

  const navigate = useNavigate();

  const handleGeoLocationConfirm = (point: LatLngTuple) => {
    setGeoLocation({
      latitude: point[0],
      longitude: point[1],
    });
    toast.success("Location selected successfully!");
  };

  const handleFormSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!validate()) {
      return;
    }

    const formData = getFormData();

    if (imagePreview.imageFile !== null) {
      formData.append("imageFile", imagePreview.imageFile);
    }

    if (geoLocation) {
      formData.append("geoLocation.latitude", geoLocation.latitude.toString());
      formData.append(
        "geoLocation.longitude",
        geoLocation.longitude.toString()
      );
    }

    try {
      CreateAccommodation(formData, {
        onSuccess: () => {
          toast.success("Accommodation created successfully!");
          reset();
          navigate("/accommodations");
        },
        onError: (error: any) => {
          console.error("Error submitting form:", error);
          toast.error("Failed to create accommodation.");
        },
      });
    } catch (error: any) {
      console.error("Error submitting form:", error);
    }
  };

  return (
    <>
      <FormHeader title="Create new Accommodation" />
      <form onSubmit={handleFormSubmit}>
        <div className="flex flex-col md:flex-row gap-4">
          <div className="flex flex-col w-full gap-2">
            <h2 className="text-xl font-bold border-b-2 py-2 mb-4">
              General information
            </h2>
            <Input
              placeholder="Enter name"
              label="Name"
              name="name"
              value={values.name}
              onChange={handleChange}
              errorMessage={errors.name}
            />
            <Input
              placeholder="Enter location"
              label="Location"
              name="location"
              value={values.location}
              onChange={handleChange}
              errorMessage={errors.location}
            />
            <Input
              placeholder="Enter description"
              label="Description"
              name="description"
              value={values.description}
              onChange={handleChange}
              errorMessage={errors.description}
            />
            <Input
              placeholder="Enter price"
              label="Price"
              name="price"
              type="number"
              value={values.price}
              onChange={handleChange}
              errorMessage={errors.price}
            />
            <Input
              placeholder="Enter number of beds"
              label="Bed Amount"
              name="bedAmount"
              type="number"
              value={values.bedAmount}
              onChange={handleChange}
              errorMessage={errors.bedAmount}
            />
          </div>

          <div className="w-full md:w-1/3 flex flex-col">
            <h2 className="text-xl font-bold border-b-2 py-2 mb-4">
              Accommodation Photo & Location
            </h2>
            <MapModal onConfirm={handleGeoLocationConfirm} />
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
        <SubmitButton
          isPending={isCreating}
          isSuccess={isSuccessCreating}
          onSubmit={(e) => handleFormSubmit(e)}
        />
      </form>
    </>
  );
};

export default CreateAccommodationForm;
