import React, { FC, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useResumeForm } from "src/hooks/useResumeForm";

import { useCart, useRemoveFromCart } from "src/api/Cart";

import { CartItem } from "src/types/Cart/CartItem";
import StepperNavigation from "src/components/Resume/StepperNavigation";
import toast from "react-hot-toast";
import { useCreateReservation } from "src/api/ResumeAPI";
import SimpleForm from "src/components/Forms/SimpleForm";
import ReviewAndSubmit from "src/components/Resume/ReviewAndSubmit";

const steps = [
  { id: 1, label: "Personal Details" },
  { id: 2, label: "Address Details" },
  { id: 3, label: "Payment Information" },
  { id: 4, label: "Review & Submit" },
];

const Resume: FC = () => {
  const initialValues = {
    firstName: "",
    lastName: "",
    email: "",
    phoneNumber: "",
    address: "",
    city: "",
    country: "",
    postalCode: "",
    cardNumber: "",
    expirationDate: "",
    cvv: "",
  };

  const { values, errors, handleChange, validate } =
    useResumeForm(initialValues);
  const [currentStep, setCurrentStep] = useState(1);

  const { data: apiCart, isError } = useCart();
  const { mutate: removeFromCart } = useRemoveFromCart();
  const { mutate: createReservation, isPending: isCreatingReservation } =
    useCreateReservation();

  const [cart, setCart] = useState<CartItem[]>([]);
  const navigate = useNavigate();

  const total = cart
    .reduce(
      (sum, item) => sum + (item.destination?.price || 0) * item.quantity,
      0
    )
    .toFixed(2);

  useEffect(() => {
    if (apiCart && Array.isArray(apiCart)) {
      setCart(apiCart);
    } else if (isError) {
      console.error("Failed to load cart data.");
    }
  }, [apiCart, isError]);

  const handleRemove = (id: string) => {
    removeFromCart(id, {
      onSuccess: () => setCart((prev) => prev.filter((item) => item.id !== id)),
      onError: (error: any) => console.error("Failed to remove item:", error),
    });
  };

  const handleNextStep = () => {
    if (currentStep === steps.length) {
      if (!validate()) {
        return;
      }

      const cartData = JSON.stringify(cart);

      createReservation(cartData, {
        onSuccess: (data) => {
          console.log("Reservation created:", data);
          toast.success("Reservation created successfully!");
          navigate(`/payment/${data.id}`);
        },
        onError: (error: any) => {
          console.error("Failed to create reservation:", error);
          toast.error("Failed to create reservation. Please try again.");
        },
      });
    } else {
      setCurrentStep((prev) => Math.min(prev + 1, steps.length));
    }
  };

  const handlePreviousStep = () => {
    setCurrentStep((prev) => Math.max(prev - 1, 1));
  };

  const stepFields = [
    [
      {
        name: "firstName",
        label: "First Name",
        type: "text",
        placeholder: "Enter your first name",
      },
      {
        name: "lastName",
        label: "Last Name",
        type: "text",
        placeholder: "Enter your last name",
      },
      {
        name: "email",
        label: "Email",
        type: "email",
        placeholder: "Enter your email",
      },
      {
        name: "phoneNumber",
        label: "Phone Number",
        type: "tel",
        placeholder: "Enter your phone number",
      },
    ],
    [
      {
        name: "address",
        label: "Address",
        type: "text",
        placeholder: "Enter your address",
      },
      {
        name: "city",
        label: "City",
        type: "text",
        placeholder: "Enter your city",
      },
      {
        name: "country",
        label: "Country",
        type: "text",
        placeholder: "Enter your country",
      },
      {
        name: "postalCode",
        label: "Postal Code",
        type: "text",
        placeholder: "Enter your postal code",
      },
    ],
    [
      {
        name: "cardNumber",
        label: "Card Number",
        type: "text",
        placeholder: "Enter your card number",
      },
      {
        name: "expirationDate",
        label: "Expiration Date",
        type: "text",
        placeholder: "MM/YY",
      },
      { name: "cvv", label: "CVV", type: "text", placeholder: "Enter CVV" },
    ],
  ];

  return (
    <div className="flex flex-col gap-4">
      <StepperNavigation steps={steps} currentStep={currentStep} />

      <div className="mb-6">
        {currentStep <= stepFields.length && (
          <SimpleForm
            fields={stepFields[currentStep - 1]}
            formData={values}
            errors={errors}
            handleInputChange={handleChange}
          />
        )}
        {currentStep === 4 && (
          <ReviewAndSubmit
            formData={values}
            errors={errors}
            cart={cart}
            total={total}
            onRemove={handleRemove}
          />
        )}
      </div>

      <div className="flex justify-between">
        <button
          className="px-4 py-2 text-white bg-gray-500 rounded-md hover:bg-gray-600"
          onClick={handlePreviousStep}
          disabled={currentStep === 1}
        >
          Back
        </button>
        <button
          className="px-4 py-2 text-white bg-blue-500 rounded-md hover:bg-blue-600"
          onClick={handleNextStep}
          disabled={isCreatingReservation}
        >
          {currentStep === steps.length
            ? isCreatingReservation
              ? "Processing..."
              : "Finish"
            : "Next"}
        </button>
      </div>
    </div>
  );
};

export default Resume;
