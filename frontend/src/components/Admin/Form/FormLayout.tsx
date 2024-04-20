import { FC } from "react";

interface FormLayoutProps {
  action?: (e: React.FormEvent) => Promise<void>;
  children: React.ReactNode;
}

const FormLayout: FC<FormLayoutProps> = ({ action, children }) => {
  return (
    <form
      className="bg-secondary rounded-lg p-4 flex flex-col grow   "
      encType="multipart/form-data"
      method="POST"
    >
      {children}
    </form>
  );
};

export default FormLayout;
