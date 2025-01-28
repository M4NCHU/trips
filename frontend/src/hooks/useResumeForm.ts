import { useState } from "react";
import { z, ZodSchema, ZodError } from "zod";

export const ResumeFormValidator = z.object({
  firstName: z.string().min(2, "First name is required").max(100),
  lastName: z.string().min(2, "Last name is required").max(100),
  email: z.string().email("Invalid email address"),
  phoneNumber: z.string().min(5, "Phone number is required"),
  address: z.string().min(5, "Address is required"),
  city: z.string().optional(),
  country: z.string().optional(),
  postalCode: z.string().optional(),
  cardNumber: z.string().min(16, "Card number is invalid"),
  expirationDate: z.string().min(5, "Expiration date is required"),
  cvv: z.string().min(3, "CVV is invalid"),
});

type FormData = z.infer<typeof ResumeFormValidator>;

export const useResumeForm = (initialValues: FormData) => {
  const [values, setValues] = useState<FormData>(initialValues);
  const [errors, setErrors] = useState<Partial<Record<keyof FormData, string>>>(
    {}
  );

  const validate = () => {
    try {
      ResumeFormValidator.parse(values);
      setErrors({});
      return true;
    } catch (error) {
      if (error instanceof ZodError) {
        const newErrors: Partial<Record<keyof FormData, string>> = {};
        error.errors.forEach((err) => {
          if (err.path.length) {
            newErrors[err.path[0] as keyof FormData] = err.message;
          }
        });
        setErrors(newErrors);
      }
      return false;
    }
  };

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    const { name, value } = e.target;
    setValues((prev) => ({ ...prev, [name]: value }));
    if (errors[name as keyof FormData]) {
      setErrors((prev) => ({ ...prev, [name]: undefined }));
    }
  };

  const reset = () => {
    setValues(initialValues);
    setErrors({});
  };

  return {
    values,
    errors,
    handleChange,
    validate,
    reset,
  };
};
