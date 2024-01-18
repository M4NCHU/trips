import { FC, useState } from "react";
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from "../ui/alert-dialog";
import { CiCirclePlus } from "react-icons/ci";
import Input from "../Forms/Input";
import { TripParticipant } from "../../types/TripParticipantTypes";
import { UseCreateTripParticipant } from "../../api/TripParticipantAPI";
// import { isDate, format } from "date-fns";

interface CreateTripParticipantModalProps {}

const initialFieldValues: TripParticipant = {
  id: 0,
  firstName: "",
  lastName: "",
  dateOfBirth: "",
  email: "",
  phoneNumber: "",
  address: "",
  emergencyContactName: "",
  emergencyContactPhone: "",
  medicalConditions: "",
  createdAt: "",
  photoUrl: "",
  imageFile: null,
  tripId: 0,
};

const CreateTripParticipantModal: FC<
  CreateTripParticipantModalProps
> = ({}) => {
  const [values, setValues] = useState<TripParticipant>(initialFieldValues);

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

  const handleFormSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const currentDate = new Date();
    let dateOfBirth: Date;

    // if (isDate(values.dateOfBirth)) {
    //     dateOfBirth = values.dateOfBirth;
    //   } else {
    //     dateOfBirth = new Date(values.dateOfBirth);
    //   }

    // // Sformatuj dateOfBirth
    // const formattedDateOfBirth = format(
    //   dateOfBirth,
    //   "yyyy-MM-dd HH:mm:ss.SSSSSSZxx"
    // );

    const formData = new FormData();
    formData.append("firstName", values.firstName);
    formData.append("lastName", values.lastName);
    formData.append("dateOfBirth", values.dateOfBirth);
    formData.append("email", values.email);
    formData.append("phoneNumber", values.phoneNumber);
    formData.append("address", values.address);
    formData.append("emergencyContactName", values.emergencyContactName);
    formData.append("emergencyContactPhone", values.emergencyContactPhone);
    formData.append("medicalConditions", values.medicalConditions);
    formData.append("photoUrl", values.photoUrl || "");
    formData.append("tripId", String(1));

    if (values.imageFile !== null && values.imageFile !== undefined) {
      formData.append("imageFile", values.imageFile);
    }

    try {
      // Assuming createDestination is a function to send the form data to the backend
      await UseCreateTripParticipant(formData);
      setValues(initialFieldValues);
    } catch (error) {
      console.error("Error submitting form:", error);
    }
  };

  return (
    <AlertDialog>
      <AlertDialogTrigger>
        <button className=" w-full p-2 flex items-center justify-center border-4 rounded-xl border-secondary hover:bg-secondary mt-2">
          <CiCirclePlus className="text-4xl font-bold" />
        </button>
      </AlertDialogTrigger>
      <AlertDialogContent>
        <div className="flex flex-col flex-wrap gap-4">
          <div className="modal-header">
            <h1 className="text-xl font-bold mb-4">Create trip participant</h1>
            <hr />
          </div>
          <form onSubmit={handleFormSubmit} className="">
            <div className="flex flex-col md:flex-row gap-4">
              <div className=" w-1/2 flex flex-col gap-4">
                <Input
                  placeholder="Enter first name"
                  label="First Name"
                  name="firstName"
                  type="text"
                  value={values.firstName}
                  onChange={handleInputChange}
                  className="p-2 rounded-lg bg-secondary"
                />
                <Input
                  placeholder="Enter last name"
                  label="Last Name"
                  name="lastName"
                  value={values.lastName}
                  onChange={handleInputChange}
                  className="p-2 rounded-lg bg-secondary"
                />
                <Input
                  placeholder="Enter date of birth"
                  label="Date of Birth"
                  name="dateOfBirth"
                  type="date"
                  value={values.dateOfBirth}
                  onChange={handleInputChange}
                  className="p-2 rounded-lg bg-secondary"
                />
                <Input
                  placeholder="Enter email"
                  label="Email"
                  name="email"
                  value={values.email}
                  onChange={handleInputChange}
                  className="p-2 rounded-lg bg-secondary"
                />
                <Input
                  placeholder="Enter phone number"
                  label="Phone Number"
                  name="phoneNumber"
                  value={values.phoneNumber}
                  onChange={handleInputChange}
                  className="p-2 rounded-lg bg-secondary"
                />
              </div>

              <div className=" w-1/2 flex flex-col gap-4">
                <Input
                  placeholder="Enter address"
                  label="Address"
                  name="address"
                  value={values.address}
                  onChange={handleInputChange}
                  className="p-2 rounded-lg bg-secondary"
                />
                <Input
                  placeholder="Enter emergency contact name"
                  label="Emergency Contact Name"
                  name="emergencyContactName"
                  value={values.emergencyContactName}
                  onChange={handleInputChange}
                  className="p-2 rounded-lg bg-secondary"
                />
                <Input
                  placeholder="Enter emergency contact phone"
                  label="Emergency Contact Phone"
                  name="emergencyContactPhone"
                  value={values.emergencyContactPhone}
                  onChange={handleInputChange}
                  className="p-2 rounded-lg bg-secondary"
                />
                <Input
                  placeholder="Enter medical conditions"
                  label="Medical Conditions"
                  name="medicalConditions"
                  value={values.medicalConditions}
                  onChange={handleInputChange}
                  className="p-2 rounded-lg bg-secondary"
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
                {values.photoUrl && (
                  <div className="img-preview">
                    <p>Image Preview</p>
                    <img src={values.photoUrl} alt="" />
                  </div>
                )}
              </div>
            </div>

            <div className="w-full flex justify-end mt-4 gap-4">
              <AlertDialogCancel className="p-4">Cancel</AlertDialogCancel>
              <button
                type="submit"
                className="bg-pink-600 rounded-lg min-w-[8rem]"
              >
                Submit
              </button>
            </div>
          </form>
        </div>
      </AlertDialogContent>
    </AlertDialog>
  );
};

export default CreateTripParticipantModal;
