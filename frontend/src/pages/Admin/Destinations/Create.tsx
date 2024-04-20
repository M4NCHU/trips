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
    <FormLayout>
      <FormContent>
        <FormSection
          className="flex flex-col  gap-2 grow"
          title="General information"
        >
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
            label="location"
            name="location"
            value={values.location}
            onChange={handleChange}
            errorMessage={errors.location}
          />
          <Input
            placeholder="Enter description"
            label="description"
            name="description"
            value={values.description}
            onChange={handleChange}
            errorMessage={errors.description}
          />

          <Input
            placeholder="Enter Price"
            label="Price"
            name="price"
            type="number"
            value={values.price}
            onChange={handleChange}
            errorMessage={errors.price}
          />
        </FormSection>

        <FormSection
          className="w-full gap-2 md:w-1/3 flex flex-col"
          title="Destination photo"
        >
          <FormLabel title="Choose category" />
          <CategoriesListModal
            action={handleCategoryToggle}
            categoryId={values.categoryId}
          />
          <Input
            placeholder="Choose photo"
            label="Choose photo"
            type="file"
            name="photoUrl"
            accept="image/*"
            onChange={showPreview}
            id="image-uploader"
          />

          <FormImagePreview
            imagePreview={imagePreview.imageSrc}
            isLoading={imagePreview.isLoading}
          />
        </FormSection>
      </FormContent>
      <SubmitButton
        isPending={CreateDestinationIsPending}
        isSuccess={CreateDestinationIsSuccess}
        isLoading={imagePreview.isLoading}
        onSubmit={(e) => handleFormSubmit(e)}
        type="submit"
      />
    </FormLayout>
  );
};

export default CreateDestination;
