import { FC, useEffect, useState } from "react";
import toast from "react-hot-toast";
import SubmitButton from "src/components/ui/SubmitButton";
import useForm from "src/hooks/useForm";
import useImagePreview from "src/hooks/useImagePreview";
import { useRoleChecker } from "src/hooks/useRoleChecker";
import { CategoryValidator } from "src/lib/validators/CategoryValidator";
import { useCreateCategory, useUpdateCategory } from "../../../api/Category";
import FormHeader from "../../Forms/FormHeader";
import Input from "../../Forms/Input";
import { useNavigate } from "react-router-dom";
import IconPicker from "src/components/Icons/IconPicker";
import { Category } from "src/types/Category"; // Zakładam, że masz taki typ danych

interface CreateCategoryFormProps {
  category?: Category | null; // Może być przekazana kategoria do edycji
  isEditMode?: boolean; // Tryb edycji
}

interface FormValues {
  name: string;
  description: string;
  icon: string;
}

const initialFieldValues: FormValues = {
  name: "",
  description: "",
  icon: "",
};

const CreateCategoryForm: FC<CreateCategoryFormProps> = ({
  category,
  isEditMode = false,
}) => {
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
    isPending: isCreatePending,
    isSuccess: isCreateSuccess,
  } = useCreateCategory();
  const {
    mutate: updateCategory,
    isPending: isUpdatePending,
    isSuccess: isUpdateSuccess,
  } = useUpdateCategory();

  const { showPreview, imagePreview } = useImagePreview();
  const [loading, setLoading] = useState(false);
  const [showCheckmark, setShowCheckmark] = useState(false);

  const navigate = useNavigate();

  useEffect(() => {
    if (isEditMode && category) {
      setValue("name", category.name);
      setValue("description", category.description);
      setValue("icon", category.icon);
    }
  }, [isEditMode, category]);

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

        if (isEditMode && category) {
          updateCategory(
            { id: category.id, formData },
            {
              onSuccess: () => {
                toast.success("Category updated successfully!");
                reset();
                navigate(0);
              },
              onError: (error: any) => {
                console.error("Error updating category:", error);
                toast.error("Failed to update category.");
              },
            }
          );
        } else {
          createCategory(formData, {
            onSuccess: () => {
              toast.success("Category created successfully!");
              setShowCheckmark(true);
              setTimeout(() => setShowCheckmark(false), 2000);
              reset();
              navigate(0);
            },
            onError: (error) => {
              console.error("Error creating category:", error);
              toast.error("Failed to create category.");
            },
          });
        }
      } catch (error) {
        console.error("Error submitting form:", error);
      }
      setLoading(false);
    }
  };

  return (
    <>
      <FormHeader
        title={isEditMode ? "Edit Category" : "Create new Category"}
      />
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
              label="Description"
              name="description"
              value={values.description}
              onChange={handleChange}
              errorMessage={errors.description}
            />

            <IconPicker setIcon={(icon) => setValue("icon", icon)} />
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
          isPending={isEditMode ? isUpdatePending : isCreatePending}
          isSuccess={isEditMode ? isUpdateSuccess : isCreateSuccess}
          onSubmit={(e) => handleFormSubmit(e)}
        />
      </form>
    </>
  );
};

export default CreateCategoryForm;
