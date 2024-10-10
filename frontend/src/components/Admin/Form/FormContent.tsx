import { FC } from "react";

interface FormContentProps {
  children: React.ReactNode;
}

const FormContent: FC<FormContentProps> = ({ children }) => {
  return <div className="flex flex-col md:flex-row gap-4 px-2">{children}</div>;
};

export default FormContent;
