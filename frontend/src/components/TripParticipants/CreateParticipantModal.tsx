import { FC, useEffect, useState } from "react";
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
import toast from "react-hot-toast";
import useForm from "src/hooks/useForm";
import SubmitButton from "../ui/SubmitButton";

interface CreateParticipantModalProps {
  tripId: string;
  onParticipantAdded: () => void;
}

export type ParticipantDTO = {
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  email: string;
  phoneNumber: string;
  address: string;
  emergencyContact: string;
  emergencyContactPhone: string;
  medicalConditions: string;
  createdAt: string;
  modifiedAt?: string | null;
  tripId: string;
};

const initialFieldValues: ParticipantDTO = {
  firstName: "",
  lastName: "",
  dateOfBirth: "",
  email: "",
  phoneNumber: "",
  address: "",
  emergencyContact: "",
  emergencyContactPhone: "",
  medicalConditions: "",
  createdAt: "2024-02-04 16:32:47.03+01",
  tripId: "",
};

const CreateParticipantModal: FC<CreateParticipantModalProps> = ({
  tripId,
  onParticipantAdded,
}) => {
  const {
    values,
    errors,
    handleChange,
    validate,
    getFormData,
    setValue,
    reset,
  } = useForm(initialFieldValues, ParticipantValidator);

  const {
    mutate: createParticipant,
    status: createParticipantStatus,
    isPending: createParticipantIsPending,
    isError: createParticipantIsError,
    isSuccess: createParticipantIsSuccess,
    error: createParticipantError,
  } = UseCreateParticipant();
  const navigate = useNavigate();

  const { showPreview, imagePreview } = useImagePreview();

  useEffect(() => {
    setValue("tripId", tripId);
  }, [tripId]);

  const handleFormSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const formData = getFormData();
    formData.append("photoUrl", imagePreview.imageSrc || "");

    if (
      imagePreview.imageFile !== null &&
      imagePreview.imageFile !== undefined
    ) {
      formData.append("imageFile", imagePreview.imageFile);
    }

    console.log(formData);
    if (validate()) {
      try {
        createParticipant(formData, {
          onSuccess: () => {
            toast.success("Trip participant created successfully!");
            onParticipantAdded();
            reset();
          },
          onError: (error: any) => {
            console.error("Error submitting form:", error);
            toast.error("Failed to create trip participant.");
          },
        });
      } catch (error) {
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
                  onChange={handleChange}
                  className="p-2 rounded-lg bg-secondary"
                  errorMessage={errors.firstName}
                />
                <Input
                  placeholder="Enter last name"
                  label="Last Name"
                  name="lastName"
                  value={values.lastName}
                  onChange={handleChange}
                  className="p-2 rounded-lg bg-secondary"
                  errorMessage={errors.lastName}
                />
                <Input
                  placeholder="Enter date of birth"
                  label="Date of Birth"
                  name="dateOfBirth"
                  type="date"
                  value={values.dateOfBirth}
                  onChange={handleChange}
                  className="p-2 rounded-lg bg-secondary"
                  errorMessage={errors.dateOfBirth}
                />
                <Input
                  placeholder="Enter email"
                  label="Email"
                  name="email"
                  value={values.email}
                  onChange={handleChange}
                  className="p-2 rounded-lg bg-secondary"
                  errorMessage={errors.email}
                />
                <Input
                  placeholder="Enter phone number"
                  label="Phone Number"
                  name="phoneNumber"
                  value={values.phoneNumber}
                  onChange={handleChange}
                  className="p-2 rounded-lg bg-secondary"
                  errorMessage={errors.phoneNumber}
                />
              </div>

              <div className="w-full md:w-1/2 flex flex-col gap-4">
                <Input
                  placeholder="Enter address"
                  label="Address"
                  name="address"
                  value={values.address}
                  onChange={handleChange}
                  className="p-2 rounded-lg bg-secondary"
                  errorMessage={errors.address}
                />
                <Input
                  placeholder="Enter emergency contact name"
                  label="Emergency Contact Name"
                  name="emergencyContact"
                  value={values.emergencyContact}
                  onChange={handleChange}
                  className="p-2 rounded-lg bg-secondary"
                  errorMessage={errors.emergencyContact}
                />
                <Input
                  placeholder="Enter emergency contact phone"
                  label="Emergency Contact Phone"
                  name="emergencyContactPhone"
                  value={values.emergencyContactPhone}
                  onChange={handleChange}
                  className="p-2 rounded-lg bg-secondary"
                  errorMessage={errors.emergencyContactPhone}
                />
                <Input
                  placeholder="Enter medical conditions"
                  label="Medical Conditions"
                  name="medicalConditions"
                  value={values.medicalConditions}
                  onChange={handleChange}
                  className="p-2 rounded-lg bg-secondary"
                  errorMessage={errors.medicalConditions}
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
              <SubmitButton
                isPending={createParticipantIsPending}
                isSuccess={createParticipantIsSuccess}
                onSubmit={(e) => handleFormSubmit(e)}
              />
            </div>
          </form>
        </div>
      </AlertDialogContent>
    </AlertDialog>
  );
};

export default CreateParticipantModal;
