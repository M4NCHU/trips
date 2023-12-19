import { FC, useState } from "react";
import { Button } from "../../../components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "../../../components/ui/card";

import FormHeader from "../../../components/Forms/FormHeader";
import Input from "../../../components/Forms/Input";
import { Link } from "react-router-dom";
import axios from "axios";
import { useMutation } from "@tanstack/react-query";

interface RegisterProps {}

interface FormValues {
  username: string;
  email: string;
  password: string;
}

const initialFieldValues: FormValues = {
  username: "",
  email: "",
  password: "",
};

const Register: FC<RegisterProps> = ({}) => {
  const [values, setValues] = useState(initialFieldValues);
  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setValues({
      ...values,
      [name]: value,
    });
  };

  const { mutate: CreateAccount } = useMutation({
    mutationFn: async () => {
      const payload = {
        username: values.username,
        email: values.email,
        password: values.password,
      };

      console.log("payload", payload);

      const { data } = await axios.post(
        "https://localhost:7154/api/Authenticate/register",
        payload
      );

      console.log(data);
      return data;
    },
  });

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
                placeholder="Enter username"
                label="Username"
                name="username"
                type="text"
                value={values.username}
                onChange={handleInputChange}
              />
              <Input
                placeholder="Enter email"
                label="Email"
                name="email"
                type="email"
                value={values.email}
                onChange={handleInputChange}
              />
              <Input
                placeholder="Enter password"
                label="Password"
                type="password"
                name="password"
                value={values.password}
                onChange={handleInputChange}
              />
            </div>
          </div>
          <Button
            className="mt-4 w-full bg-red-400 "
            onClick={() => CreateAccount()}
          >
            Register
          </Button>
          <p className="py-2 mt-2 text-foreground">
            Already have an account? <Link to="/login">Login</Link>
          </p>
        </div>
      </div>
    </div>
  );
};

export default Register;
