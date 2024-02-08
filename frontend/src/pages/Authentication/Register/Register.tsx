import { FC, useState } from "react";
import { Button } from "../../../components/ui/button";

import { Link, useNavigate } from "react-router-dom";
import { UseCreateUser } from "src/api/AuthenticationAPI";
import FormHeader from "../../../components/Forms/FormHeader";
import Input from "../../../components/Forms/Input";
import {
  UserLoginValidator,
  UserRegistrationValidator,
} from "src/lib/validators/UserValidator";
import { ZodError } from "zod";
import toast from "react-hot-toast";
import useForm from "src/hooks/useForm";
import SubmitButton from "src/components/ui/SubmitButton";

interface RegisterProps {}

interface FormValues {
  username: string;
  email: string;
  password: string;
  firstName: string;
  lastName: string;
}

const initialFieldValues: FormValues = {
  username: "",
  email: "",
  password: "",
  firstName: "",
  lastName: "",
};

const Register: FC<RegisterProps> = ({}) => {
  const { values, errors, handleChange, validate, getFormData, reset } =
    useForm(initialFieldValues, UserRegistrationValidator);

  const {
    mutate: createCategory,
    status,
    isPending,
    isError,
    isSuccess,
    error,
  } = UseCreateUser();

  const navigate = useNavigate();

  const CreateUser = async (e: React.FormEvent) => {
    e.preventDefault();
    if (validate()) {
      const formData = getFormData();
      try {
        createCategory(formData, {
          onSuccess: () => {
            toast.success("Account created successfully!, please login");
            reset();
            navigate("/login");
          },
          onError: (error) => {
            console.error("Error submitting form:", error);
            toast.error("Failed to create account.");
          },
        });
      } catch (error) {
        console.error("Error submitting form:", error);
      }
    }
  };

  return (
    <div className="container px-4">
      <FormHeader title="Register" />
      <div className="flex justify-center mt-4">
        <div className=" bg-secondary rounded-lg p-4 w-[40rem]">
          <div className="flex flex-col md:flex-row gap-4">
            <div className="flex flex-col w-full gap-2">
              <h2 className="text-xl font-bold border-b-2 py-2 mb-4 ">
                Register
              </h2>
              <Input
                placeholder="Enter first name"
                label="first name"
                name="firstName"
                type="text"
                value={values.firstName}
                onChange={handleChange}
                errorMessage={errors.firstName}
              />
              <Input
                placeholder="Enter last name"
                label="Last name"
                name="lastName"
                type="text"
                value={values.lastName}
                onChange={handleChange}
                errorMessage={errors.lastName}
              />
              <Input
                placeholder="Enter username"
                label="Username"
                name="username"
                type="text"
                value={values.username}
                onChange={handleChange}
                errorMessage={errors.username}
              />
              <Input
                placeholder="Enter email"
                label="Email"
                name="email"
                type="email"
                value={values.email}
                onChange={handleChange}
                errorMessage={errors.email}
              />
              <Input
                placeholder="Enter password"
                label="Password"
                type="password"
                name="password"
                value={values.password}
                onChange={handleChange}
                errorMessage={errors.password}
              />
            </div>
          </div>
          <SubmitButton
            isPending={isPending}
            isSuccess={isSuccess}
            onSubmit={(e) => CreateUser(e)}
          />
          <p className="py-2 mt-2 text-foreground">
            Already have an account? <Link to="/login">Login</Link>
          </p>
        </div>
      </div>
    </div>
  );
};

export default Register;
