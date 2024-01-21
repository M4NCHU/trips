import { FC, useState } from "react";
import { CiCirclePlus } from "react-icons/ci";
import { UseCreateParticipant } from "../../api/ParticipantAPI";
import { Participant } from "../../types/ParticipantTypes";
import Input from "../Forms/Input";
import {
  AlertDialog,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogTrigger,
} from "../ui/alert-dialog";
import useImagePreview from "src/hooks/useImagePreview";
import { ParticipantValidator } from "src/lib/validators/TripParticipantValidatior";
import { ZodError } from "zod";
import { useNavigate } from "react-router-dom";
import { UseCreateTripParticipant } from "src/api/TripParticipantAPI";

interface CreateParticipantModalProps {
  tripId: string;
}

const initialFieldValues: Participant = {
  id: "",
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
  tripId: "",
};

const CreateParticipantModal: FC<CreateParticipantModalProps> = ({
  tripId,
}) => {
  const [values, setValues] = useState<Participant>(initialFieldValues);
  const [validationErrors, setValidationErrors] = useState<
    Record<string, string>
  >({});
  const { showPreview, imagePreview } = useImagePreview();
  const navigate = useNavigate();

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setValues({
      ...values,
      [name]: value,
    });
  };

  const handleFormSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const formDataObject = {
      firstName: values.firstName,
      lastName: values.lastName,
      dateOfBirth: values.dateOfBirth,
      email: values.email,
      phoneNumber: values.phoneNumber.toString(),
      address: values.address,
      emergencyContactName: values.emergencyContactName,
      emergencyContactPhone: values.emergencyContactPhone,
      medicalConditions: values.medicalConditions,
    };

    const formData = new FormData();
    formData.append("firstName", values.firstName);
    formData.append("lastName", values.lastName);
    formData.append("dateOfBirth", values.dateOfBirth);
    formData.append("email", values.email);
    formData.append("phoneNumber", values.phoneNumber.toString());
    formData.append("address", values.address);
    formData.append("emergencyContact", values.emergencyContactName);
    formData.append("emergencyContactPhone", values.emergencyContactPhone);
    formData.append("medicalConditions", values.medicalConditions);
    formData.append("photoUrl", imagePreview.imageSrc || "");
    formData.append("tripId", tripId);

    if (
      imagePreview.imageFile !== null &&
      imagePreview.imageFile !== undefined
    ) {
      formData.append("imageFile", imagePreview.imageFile);
    }

    try {
      ParticipantValidator.parse(formDataObject);

      const participant = await UseCreateParticipant(formData);
      console.log(participant);

      setValues(initialFieldValues);
      setValidationErrors({});
      imagePreview.imageSrc = "";
      imagePreview.imageFile = null;
      navigate(0);
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
    <AlertDialog>
      <AlertDialogTrigger className=" w-full p-2 flex items-center justify-center border-4 rounded-xl border-secondary hover:bg-secondary mt-2">
        <CiCirclePlus className="text-4xl font-bold" />
      </AlertDialogTrigger>
      <AlertDialogContent>
        <div className="flex flex-col flex-wrap gap-4">
          <div className="modal-header">
            <h1 className="text-xl font-bold mb-4">Create trip participant</h1>
            <hr />
          </div>
          <form onSubmit={handleFormSubmit} className="">
            <div className="flex flex-col md:flex-row gap-4">
              <div className="w-full md:w-1/2 flex flex-col gap-4">
                <Input
                  placeholder="Enter first name"
                  label="First Name"
                  name="firstName"
                  type="text"
                  value={values.firstName}
                  onChange={handleInputChange}
                  className="p-2 rounded-lg bg-secondary"
                  errorMessage={validationErrors.firstName}
                />
                <Input
                  placeholder="Enter last name"
                  label="Last Name"
                  name="lastName"
                  value={values.lastName}
                  onChange={handleInputChange}
                  className="p-2 rounded-lg bg-secondary"
                  errorMessage={validationErrors.lastName}
                />
                <Input
                  placeholder="Enter date of birth"
                  label="Date of Birth"
                  name="dateOfBirth"
                  type="date"
                  value={values.dateOfBirth}
                  onChange={handleInputChange}
                  className="p-2 rounded-lg bg-secondary"
                  errorMessage={validationErrors.dateOfBirth}
                />
                <Input
                  placeholder="Enter email"
                  label="Email"
                  name="email"
                  value={values.email}
                  onChange={handleInputChange}
                  className="p-2 rounded-lg bg-secondary"
                  errorMessage={validationErrors.email}
                />
                <Input
                  placeholder="Enter phone number"
                  label="Phone Number"
                  name="phoneNumber"
                  value={values.phoneNumber}
                  onChange={handleInputChange}
                  className="p-2 rounded-lg bg-secondary"
                  errorMessage={validationErrors.phoneNumber}
                />
              </div>

              <div className="w-full md:w-1/2 flex flex-col gap-4">
                <Input
                  placeholder="Enter address"
                  label="Address"
                  name="address"
                  value={values.address}
                  onChange={handleInputChange}
                  className="p-2 rounded-lg bg-secondary"
                  errorMessage={validationErrors.address}
                />
                <Input
                  placeholder="Enter emergency contact name"
                  label="Emergency Contact Name"
                  name="emergencyContactName"
                  value={values.emergencyContactName}
                  onChange={handleInputChange}
                  className="p-2 rounded-lg bg-secondary"
                  errorMessage={validationErrors.emergencyContactName}
                />
                <Input
                  placeholder="Enter emergency contact phone"
                  label="Emergency Contact Phone"
                  name="emergencyContactPhone"
                  value={values.emergencyContactPhone}
                  onChange={handleInputChange}
                  className="p-2 rounded-lg bg-secondary"
                  errorMessage={validationErrors.emergencyContactPhone}
                />
                <Input
                  placeholder="Enter medical conditions"
                  label="Medical Conditions"
                  name="medicalConditions"
                  value={values.medicalConditions}
                  onChange={handleInputChange}
                  className="p-2 rounded-lg bg-secondary"
                  errorMessage={validationErrors.medicalConditions}
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
                {imagePreview.imageSrc && (
                  <div className="img-preview">
                    <p>Image Preview</p>
                    <img src={imagePreview.imageSrc} alt="" />
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

export default CreateParticipantModal;
