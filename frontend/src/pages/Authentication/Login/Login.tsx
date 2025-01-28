import { FC, useContext, useState } from "react";
import { Button } from "../../../components/ui/button";

import { useMutation } from "@tanstack/react-query";
import axios from "axios";
import { Link, useNavigate } from "react-router-dom";
import FormHeader from "../../../components/Forms/FormHeader";
import Input from "../../../components/Forms/Input";
import { UseLoginUser } from "src/api/AuthenticationAPI";
import { useAuth } from "src/context/UserContext";
import { UserLoginValidator } from "src/lib/validators/UserValidator";
import toast from "react-hot-toast";
import { ZodError } from "zod";
import useForm from "src/hooks/useForm";
import SubmitButton from "src/components/ui/SubmitButton";
import { LoginResponse, User } from "src/types/UserTypes";

interface LoginProps {}

interface FormValues {
  username: string;
  password: string;
}

const initialFieldValues: FormValues = {
  username: "",
  password: "",
};

const Login: FC<LoginProps> = ({}) => {
  const { setUser } = useAuth();
  const { values, errors, handleChange, validate, getFormData, reset } =
    useForm(initialFieldValues, UserLoginValidator);

  const {
    mutate: LoginUserMutation,
    status,
    isPending,
    isError,
    isSuccess,
    error,
    data,
  } = UseLoginUser();

  const navigate = useNavigate();

  const LoginUser = async (e: React.FormEvent) => {
    e.preventDefault();
    if (validate()) {
      const formData = getFormData();
      try {
        LoginUserMutation(formData, {
          onSuccess: (data: LoginResponse) => {
            if (data.token && data.user) {
              setUser({ ...data.user, token: data.token });
            }
            toast.success(`Successfully logged in!`);
            navigate("/");
          },
          onError: (error) => {
            console.error("Error submitting form:", error);
            toast.error("Failed to login.");
          },
        });
      } catch (error) {
        console.error("Error submitting form:", error);
      }
    }
  };

  return (
    <div className="container px-4">
      <FormHeader title="Login" />
      <div className="flex justify-center mt-4">
        <div className=" bg-secondary rounded-lg p-4 w-[40rem]">
          <div className="flex flex-col md:flex-row gap-4">
            <div className="flex flex-col w-full gap-2">
              <h2 className="text-xl font-bold border-b-2 py-2 mb-4 ">Login</h2>
              <Input
                className="bg-background p-3 rounded-lg"
                placeholder="Enter username"
                label="Username"
                name="username"
                type="text"
                value={values.username}
                onChange={handleChange}
                errorMessage={errors.username}
              />

              <Input
                className="bg-background p-3 rounded-lg"
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
            onSubmit={(e) => LoginUser(e)}
          />
          <p className="py-2 mt-2 text-foreground">
            You don't have an account? <Link to="/register">Register</Link>
          </p>
        </div>
      </div>
    </div>
  );
};

export default Login;
