import { FC, useState } from "react";
import { createCategory } from "../../api/Category";
import FormHeader from "../../components/Forms/FormHeader";
import Input from "../../components/Forms/Input";
import { Button } from "../../components/ui/button";

interface CreateProps {}

interface FormValues {
  categoryName: string;
  categoryDesc: string;
  photoUrl: string;
  imageFile: File | null;
}

const initialFieldValues: FormValues = {
  categoryName: "",
  categoryDesc: "",
  photoUrl: "",
  imageFile: null,
};

const CreateCategory: FC<CreateProps> = ({}) => {
  const [values, setValues] = useState(initialFieldValues);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setValues({
      ...values,
      [name]: value,
    });
  };

  const showPreview = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files[0]) {
      const imageFile: File = e.target.files[0];
      const reader = new FileReader();

      reader.onload = (x: ProgressEvent<FileReader>) => {
        if (x.target && typeof x.target.result === "string") {
          setValues({
            ...values,
            imageFile: imageFile,
            photoUrl: x.target.result,
          });
        }
      };

      reader.readAsDataURL(imageFile);
    } else {
      setValues({
        ...values,
        imageFile: null,
        photoUrl: "",
      });
    }
  };

  console.log(values.photoUrl);
  const handleFormSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const formData = new FormData();
    formData.append("name", values.categoryName);
    formData.append("description", values.categoryDesc);
    formData.append("photoUrl", values.photoUrl);

    if (values.imageFile !== null) {
      formData.append("imageFile", values.imageFile);
    }

    try {
      await createCategory(formData);
    } catch (error) {
      console.error("Error submitting form:", error);
    }
  };

  return (
    <div className="container px-4">
      <FormHeader title="Create new Category" />
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
              name="categoryName"
              value={values.categoryName}
              onChange={handleInputChange}
            />

            <Input
              placeholder="Enter description"
              label="description"
              name="categoryDesc"
              value={values.categoryDesc}
              onChange={handleInputChange}
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
            {values.photoUrl && (
              <div className="img-preview">
                <p>Image Preview</p>
                <img src={values.photoUrl} alt="" />
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

export default CreateCategory;
