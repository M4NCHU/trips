import { FC, useState } from "react";
import { UseCategoryList } from "../../../api/Category";
import { UseCreateDestination } from "../../../api/Destinations";
import FormHeader from "../../../components/Forms/FormHeader";
import Input from "../../../components/Forms/Input";
import { Button } from "../../../components/ui/button";
import { DestinationValidator } from "../../../lib/validators/DestinationValidator";
import { ZodError } from "zod";
import useImagePreview from "src/hooks/useImagePreview";
import { useNavigate } from "react-router-dom";
import useForm from "src/hooks/useForm";
import toast from "react-hot-toast";
import SubmitButton from "src/components/ui/SubmitButton";
import FormLayout from "src/components/Admin/Form/FormLayout";
import FormContent from "src/components/Admin/Form/FormContent";
import FormSection from "src/components/Admin/Form/FormSection";
import FormImagePreview from "src/components/Admin/Form/FormImagePreview";
import CategoriesListModal from "src/components/Admin/Category/CategoriesListModal";
import FormTitle from "src/components/Admin/Form/FormTitle";
import FormLabel from "src/components/Admin/Form/FormLabel";
import CreateDestinationModal from "src/components/Destinations/Modals/CreateDestinationModal";
import CreateDestinationForm from "src/components/Destinations/Forms/CreateDestinationForm";

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
    isPending: CreateDestinationIsPending,
    isSuccess: CreateDestinationIsSuccess,
  } = UseCreateDestination();

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
