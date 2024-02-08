import { FC } from "react";
import { FaArrowLeft } from "react-icons/fa6";
import { Link } from "react-router-dom";

interface FormHeaderProps {
  title: string;
  backLink?: string;
  backText?: string;
}

const FormHeader: FC<FormHeaderProps> = ({ title, backLink, backText }) => {
  return (
    <div className="form-header flex flex-row py-6 gap-4 items-center">
      <Link
        to={`${backLink ? backLink : "/"}`}
        className="bg-transparent p-3 rounded-lg text-foreground border-2 border-secondary hover:bg-secondary"
      >
        <FaArrowLeft />
      </Link>
      <div className="flex flex-col justify-center">
        <span className="text-xs">
          {backText ? backText : "Back to home page"}
        </span>
        <h1 className="text-2xl font-bold">{title}</h1>
      </div>
    </div>
  );
};

export default FormHeader;
