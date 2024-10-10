import { FC } from "react";
import toast from "react-hot-toast";
import { useNavigate } from "react-router-dom";
import SubmitButton from "src/components/ui/SubmitButton";
import useForm from "src/hooks/useForm";
import useImagePreview from "src/hooks/useImagePreview";
import { UseCategoryList } from "../../../api/Category";
import { UseCreateDestination } from "../../../api/Destinations";
import { DestinationValidator } from "../../../lib/validators/DestinationValidator";
import FormHeader from "../../Forms/FormHeader";
import Input from "../../Forms/Input";

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

const CreateDestinationForm: FC<CreateProps> = ({}) => {
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
  const { data: categories } = UseCategoryList();

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
            navigate(0);
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
      <FormHeader title="Create new Destination" />
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

            <div className="flex flex-col gap-4 mb-2">
              <label className="font-bold mb-2" htmlFor="">
                Choose category
              </label>

              <div>
                {categories
                  ? categories.map((category, i) => (
                      <div
                        key={category.id}
                        className={`flex flex-col w-14 h-14 items-center justify-center relative gap-1 ${
                          values.categoryId === category.id
                            ? "border-2 border-blue-500"
                            : ""
                        }`}
                        onClick={() => handleCategoryToggle(category.id)}
                      >
                        <img
                          src={category.photoUrl}
                          alt={`${category.name}`}
                          className="object-cover"
                        />
                        <div>{category.name}</div>
                      </div>
                    ))
                  : "There is no categories"}
              </div>
            </div>
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
            {imagePreview.imageSrc && (
              <div className="img-preview">
                <p>Image Preview</p>
                <img src={imagePreview.imageSrc} alt="" />
              </div>
            )}
          </div>
        </div>
        <SubmitButton
          isPending={CreateDestinationIsPending}
          isSuccess={CreateDestinationIsSuccess}
          onSubmit={(e) => handleFormSubmit(e)}
        />
      </form>
    </>
  );
};

export default CreateDestinationForm;
