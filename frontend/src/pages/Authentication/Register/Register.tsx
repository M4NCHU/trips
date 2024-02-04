import { FC, useState } from "react";
import { Button } from "../../../components/ui/button";

import { Link } from "react-router-dom";
import { UseCreateUser } from "src/api/AuthenticationAPI";
import FormHeader from "../../../components/Forms/FormHeader";
import Input from "../../../components/Forms/Input";

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
  const [values, setValues] = useState(initialFieldValues);
  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setValues({
      ...values,
      [name]: value,
    });
  };

  const CreateUser = async () => {
    const formData = new FormData();
    formData.append("firstName", values.firstName);
    formData.append("lastName", values.lastName);
    formData.append("username", values.username);
    formData.append("email", values.email);
    formData.append("password", values.password);

    try {
      await UseCreateUser(formData);
    } catch (error: any) {
      console.error("Error submitting form:", error);
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
                onChange={handleInputChange}
              />
              <Input
                placeholder="Enter last name"
                label="Last name"
                name="lastName"
                type="text"
                value={values.lastName}
                onChange={handleInputChange}
              />
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
            onClick={() => CreateUser()}
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
