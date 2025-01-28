import { useState, ChangeEvent } from "react";
import toast from "react-hot-toast";
import { ZodError, ZodSchema } from "zod";

type UseForm<T> = {
  values: T;
  errors: Record<keyof T, string>;
  handleChange: (e: ChangeEvent<HTMLInputElement>) => void;
  validate: () => boolean;
  getFormData: () => FormData;
  setValue: (name: keyof T, value: any) => void;
  reset: () => void;
};

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

    if (errors[name]) {
      setErrors({
        ...errors,
        [name]: "",
      });
    }
  };

  const setValue = (name: keyof T, value: any) => {
    setValues((prevValues) => ({
      ...prevValues,
      [name]: value,
    }));
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
        toast.error("Some error occurred!");
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

  const reset = () => {
    setValues(initialValues);
    setErrors({} as Record<keyof T, string>);
  };

  return {
    values,
    errors,
    handleChange,
    validate,
    getFormData,
    setValue,
    reset,
  };
};

export default useForm;
