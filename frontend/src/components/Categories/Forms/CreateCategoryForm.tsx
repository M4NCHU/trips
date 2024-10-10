import { FC, useState } from "react";
import { UseCategoryList, useCreateCategory } from "../../../api/Category";
import { UseCreateDestination } from "../../../api/Destinations";
import FormHeader from "../../Forms/FormHeader";
import Input from "../../Forms/Input";
import { Button } from "../../ui/button";
import { DestinationValidator } from "../../../lib/validators/DestinationValidator";
import { ZodError } from "zod";
import useImagePreview from "src/hooks/useImagePreview";
import { useNavigate } from "react-router-dom";
import useForm from "src/hooks/useForm";
import toast from "react-hot-toast";
import SubmitButton from "src/components/ui/SubmitButton";
import { useRoleChecker } from "src/hooks/useRoleChecker";
import { CategoryValidator } from "src/lib/validators/CategoryValidator";

interface CreateProps {}

interface FormValues {
  name: string;
  description: string;
}

const initialFieldValues: FormValues = {
  name: "",
  description: "",
};

const CreateCategoryForm: FC<CreateProps> = ({}) => {
  const {
    values,
    errors,
    handleChange,
    validate,
    getFormData,
    setValue,
    reset,
  } = useForm(initialFieldValues, CategoryValidator);
  const {
    mutate: createCategory,
    status,
    isPending,
    isError,
    isSuccess,
    error,
  } = useCreateCategory();

  const { showPreview, imagePreview } = useImagePreview();
  const { hasRole } = useRoleChecker();
  const [loading, setLoading] = useState(false);
  const [showCheckmark, setShowCheckmark] = useState(false);
  const handleFormSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (validate()) {
      setLoading(true);
      try {
        const formData = getFormData();
        formData.append("photoUrl", imagePreview.imageSrc || "");

        if (
          imagePreview.imageFile !== null &&
          imagePreview.imageFile !== undefined
        ) {
          formData.append("imageFile", imagePreview.imageFile);
        }

        console.log(formData);
        createCategory(formData, {
          onSuccess: () => {
            toast.success("Category created successfully!");
            setShowCheckmark(true);
            setTimeout(() => setShowCheckmark(false), 2000);
            reset();
          },
          onError: (error) => {
            console.error("Error submitting form:", error);
            toast.error("Failed to create category.");
          },
        });
      } catch (error) {
        console.error("Error submitting form:", error);
      }
      setLoading(false);
    }
  };

  return (
    <>
      <FormHeader title="Create new Category" />
      <form onSubmit={handleFormSubmit}>
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
              label="description"
              name="description"
              value={values.description}
              onChange={handleChange}
              errorMessage={errors.description}
            />
          </div>

          <div className="w-full md:w-1/3 flex flex-col">
            <h2 className="text-xl font-bold border-b-2 py-2 mb-4">
              Category photo
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
          isPending={isPending}
          isSuccess={isSuccess}
          onSubmit={(e) => handleFormSubmit(e)}
        />
      </form>
    </>
  );
};

export default CreateCategoryForm;
