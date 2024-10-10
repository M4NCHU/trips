import { FC } from "react";
import toast from "react-hot-toast";
import { useNavigate } from "react-router-dom";
import CreateDestinationForm from "src/components/Destinations/Forms/CreateDestinationForm";
import useForm from "src/hooks/useForm";
import useImagePreview from "src/hooks/useImagePreview";
import { UseCategoryList } from "../../api/Category";
import { UseCreateDestination } from "../../api/Destinations";
import { DestinationValidator } from "../../lib/validators/DestinationValidator";
import CreateDestinationModal from "src/components/Destinations/Modals/CreateDestinationModal";

interface CreateProps {}

interface FormValues {
  name: string;
  description: string;
  location: string;
  categoryId: string;
  price: string;
}

const initialFieldValues: FormValues = {
  name: "",
  description: "",
  location: "",
  categoryId: "",
  price: "",
};

const CreateDestination: FC<CreateProps> = ({}) => {
  const {
    values,
    errors,
    handleChange,
    validate,
    getFormData,
    setValue,
    reset,
  } = useForm(initialFieldValues, DestinationValidator);
  const { showPreview, imagePreview } = useImagePreview();

  const {
    mutate: CreateDestination,
    status: CreateDestinationStatus,
    isPending: CreateDestinationIsPending,
    isError: CreateDestinationIsError,
    isSuccess: CreateDestinationIsSuccess,
    error: CreateDestinationError,
    data: destinationData,
  } = UseCreateDestination();
  const { data: categories, isLoading, isError } = UseCategoryList();

  const navigate = useNavigate();

  const handleCategoryToggle = (categoryId: string) => {
    setValue("categoryId", categoryId);
  };

  const handleFormSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const formData = getFormData();

    if (imagePreview.imageFile !== null) {
      formData.append("imageFile", imagePreview.imageFile);
    }

    if (validate()) {
      try {
        CreateDestination(formData, {
          onSuccess: (destinationData) => {
            toast.success("Destination created successfully!");
            reset();
          },
          onError: (error: any) => {
            console.error("Error submitting form:", error);
            toast.error("Failed to create destination.");
          },
        });
      } catch (error) {
        console.error("Error submitting form:", error);
      }
    }
  };

  return (
    <>
      <CreateDestinationModal />
      <CreateDestinationForm />
    </>
  );
};

export default CreateDestination;
