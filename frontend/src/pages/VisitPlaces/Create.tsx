import { FC, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useDestinationById } from "../../api/Destinations";
import FormHeader from "../../components/Forms/FormHeader";
import Input from "../../components/Forms/Input";
import { Button } from "../../components/ui/button";
import useImagePreview from "src/hooks/useImagePreview";
import useForm from "src/hooks/useForm";
import { VisitPlaceValidator } from "src/lib/validators/VisitPlaceValidator";
import toast from "react-hot-toast";
import { UseCreateVisitPlace } from "src/api/VisitPlaceAPI";
import SubmitButton from "src/components/ui/SubmitButton";

interface CreateProps {}

interface FormValues {
  name: string;
  description: string;
  price: string;
  destinationId: string;
}

const initialFieldValues: FormValues = {
  name: "",
  description: "",
  price: "",
  destinationId: "",
};

const CreateVisitPlace: FC<CreateProps> = ({}) => {
  const { id } = useParams<{ id: string | undefined }>();
  const {
    values,
    errors,
    handleChange,
    validate,
    getFormData,
    setValue,
    reset,
  } = useForm(initialFieldValues, VisitPlaceValidator);
  const {
    mutate: createCategory,
    status: createCategoryStatus,
    isPending: createCategoryIsPending,
    isError: createCategoryIsError,
    isSuccess: createCategoryIsSuccess,
    error: createCategoryError,
  } = UseCreateVisitPlace();
  const navigate = useNavigate();
  const { data: destination, isLoading, isError } = useDestinationById(id);
  const { showPreview, imagePreview } = useImagePreview();

  useEffect(() => {
    setValue("destinationId", id);
  }, [destination]);

  const handleFormSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!destination) {
      console.error("Error: No destination selected.");
      alert(
        "No destination selected. You cannot add a visit place without a destination."
      );
      return;
    }
    const formData = getFormData();
    formData.append("photoUrl", imagePreview.imageSrc || "");

    if (
      imagePreview.imageFile !== null &&
      imagePreview.imageFile !== undefined
    ) {
      formData.append("imageFile", imagePreview.imageFile);
    }

    console.log(formData);
    if (validate()) {
      try {
        createCategory(formData, {
          onSuccess: () => {
            toast.success("Category created successfully!");
            reset();
            navigate(`/destination/${destination?.id}`);
          },
          onError: (error: any) => {
            console.error("Error submitting form:", error);
            toast.error("Failed to create category.");
          },
        });
        reset();
      } catch (error) {
        console.error("Error submitting form:", error);
      }
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
      <FormHeader
        title="Create new visit place"
        backLink={`/destination/${destination?.id}`}
        backText={`Back to ${destination.name}`}
      />
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
              name="name"
              value={values.name}
              onChange={handleChange}
              errorMessage={errors.name}
            />
            <Input
              placeholder="Enter description"
              label="Description"
              name="description"
              value={values.description}
              onChange={handleChange}
              errorMessage={errors.name}
            />
            <Input
              placeholder="Enter description"
              label="Price/person"
              name="price"
              value={values.price}
              onChange={handleChange}
              errorMessage={errors.name}
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
        <SubmitButton
          isPending={createCategoryIsPending}
          isSuccess={createCategoryIsSuccess}
          onSubmit={(e) => handleFormSubmit(e)}
        />
      </form>
    </div>
  );
};

export default CreateVisitPlace;
