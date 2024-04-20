import { FC } from "react";

interface FormContentProps {
  children: React.ReactNode;
}

const FormContent: FC<FormContentProps> = ({ children }) => {
  return (
    <div className="flex h-1 flex-col md:flex-row gap-4 px-2 grow overflow-auto">
      {children}
    </div>
  );
};

export default FormContent;
