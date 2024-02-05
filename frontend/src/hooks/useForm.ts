import { useState, ChangeEvent } from "react";
import { ZodError, ZodSchema } from "zod";

// Define a generic type for the useForm hook
type UseForm<T> = {
  values: T;
  errors: Record<keyof T, string>;
  handleChange: (e: ChangeEvent<HTMLInputElement>) => void;
  validate: () => boolean;
  getFormData: () => FormData;
};

// useForm hook
const useForm = <T extends Record<string, any>>(
  initialValues: T,
  validator: ZodSchema<T>
): UseForm<T> => {
  const [values, setValues] = useState<T>(initialValues);
  const [errors, setErrors] = useState<Record<keyof T, string>>(
    {} as Record<keyof T, string>
  );

  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setValues({
      ...values,
      [name]: value,
    });
    // Optionally clear validation error for a field when it's edited
    if (errors[name]) {
      setErrors({
        ...errors,
        [name]: "",
      });
    }
  };

  const validate = () => {
    try {
      validator.parse(values);
      setErrors({} as Record<keyof T, string>);
      return true;
    } catch (error) {
      if (error instanceof ZodError) {
        const newErrors: Record<keyof T, string> = {} as Record<
          keyof T,
          string
        >;
        for (const issue of error.issues) {
          if (typeof issue.path[0] === "string") {
            newErrors[issue.path[0] as keyof T] = issue.message;
          }
        }
        setErrors(newErrors);
      }
      return false;
    }
  };

  const getFormData = () => {
    const formData = new FormData();
    for (const key in values) {
      formData.append(key, values[key]);
    }
    return formData;
  };

  return {
    values,
    errors,
    handleChange,
    validate,
    getFormData,
  };
};

export default useForm;
