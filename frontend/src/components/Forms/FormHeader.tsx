import { FC } from "react";
import { FaArrowLeft } from "react-icons/fa6";
import { Button } from "../ui/button";

interface FormHeaderProps {
  title: string;
}

const FormHeader: FC<FormHeaderProps> = ({ title }) => {
  return (
    <div className="form-header flex flex-row py-6 gap-4 items-center">
      <Button className="bg-transparent text-foreground border-2 border-secondary hover:bg-secondary">
        <FaArrowLeft />
      </Button>
      <div className="flex flex-col justify-center">
        <span className="text-xs">Back to home page</span>
        <h1 className="text-2xl font-bold">{title}</h1>
      </div>
    </div>
  );
};

export default FormHeader;
