import { FC, useState } from "react";
import { UseCategoryList } from "../../api/Category";
import { UseCreateDestination } from "../../api/Destinations";
import FormHeader from "../../components/Forms/FormHeader";
import Input from "../../components/Forms/Input";
import { Button } from "../../components/ui/button";
import { DestinationValidator } from "../../lib/validators/destination";
import { ZodError } from "zod";
import useImagePreview from "src/hooks/useImagePreview";

interface CreateProps {}

interface FormValues {
  destinationName: string;
  destinationLocation: string;
  destinationDesc: string;
  imageSrc: string;
  imageFile: File | null;
  selectedCategory: string;
  price: string;
}

const initialFieldValues: FormValues = {
  destinationName: "",
  destinationLocation: "",
  destinationDesc: "",
  imageSrc: "",
  imageFile: null,
  selectedCategory: "",
  price: "",
};

const CreateDestination: FC<CreateProps> = ({}) => {
  const [values, setValues] = useState(initialFieldValues);
  const { data: categories, isLoading, isError } = UseCategoryList();
  const [validationErrors, setValidationErrors] = useState<
    Record<string, string>
  >({});
  const { showPreview, imagePreview } = useImagePreview();

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setValues({
      ...values,
      [name]: value,
    });
  };

  const handleCategoryToggle = (categoryId: string) => {
    setValues({
      ...values,
      selectedCategory:
        values.selectedCategory === categoryId ? "0" : categoryId,
    });
  };

  const handleFormSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const formDataObject = {
      name: values.destinationName,
      description: values.destinationDesc,
      location: values.destinationLocation,
      price: parseFloat(values.price),
      categoryId: values.selectedCategory,
    };

    const formData = new FormData();
    formData.append("name", values.destinationName);
    formData.append("location", values.destinationLocation);
    formData.append("description", values.destinationDesc);
    formData.append("photoUrl", imagePreview.imageSrc);
    formData.append("categoryId", String(values.selectedCategory));
    formData.append("price", values.price);

    if (imagePreview.imageFile !== null) {
      formData.append("imageFile", imagePreview.imageFile);
    }

    try {
      DestinationValidator.parse(formDataObject);
      await UseCreateDestination(formData);
    } catch (error) {
      if (error instanceof ZodError) {
        const newErrors: Record<string, string> = {};
        for (const issue of error.issues) {
          newErrors[issue.path[0]] = issue.message;
        }
        setValidationErrors(newErrors);
      } else {
        console.error("Error submitting form:", error);
      }
    }
  };

  return (
    <div className="container px-4">
      <FormHeader title="Create new Destination" />
      <form
        className=" bg-secondary rounded-lg p-4 "
        onSubmit={handleFormSubmit}
      >
        <div className="flex flex-col md:flex-row gap-4">
          <div className="flex flex-col w-full gap-2">
            <h2 className="text-xl font-bold border-b-2 py-2 mb-4 ">
              General information
            </h2>
            <Input
              placeholder="Enter name"
              label="Name"
              name="destinationName"
              value={values.destinationName}
              onChange={handleInputChange}
              errorMessage={validationErrors.name}
            />
            <Input
              placeholder="Enter location"
              label="location"
              name="destinationLocation"
              value={values.destinationLocation}
              onChange={handleInputChange}
              errorMessage={validationErrors.location}
            />
            <Input
              placeholder="Enter description"
              label="description"
              name="destinationDesc"
              value={values.destinationDesc}
              onChange={handleInputChange}
              errorMessage={validationErrors.description}
            />

            <Input
              placeholder="Enter Price"
              label="Price"
              name="price"
              type="number"
              value={values.price}
              onChange={handleInputChange}
              errorMessage={validationErrors.price}
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
                          values.selectedCategory === category.id
                            ? "border-2 border-blue-500" // Apply styling for the selected category
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
        <Button className="mt-4 w-full bg-red-400 " type="submit">
          Create +
        </Button>
      </form>
    </div>
  );
};

export default CreateDestination;
