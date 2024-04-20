import { FC } from "react";

interface FormTitleProps {
  title: string;
}

const FormTitle: FC<FormTitleProps> = ({ title }) => {
  return <h2 className="text-xl font-bold border-b-2 py-2 mb-4">{title}</h2>;
};

export default FormTitle;
