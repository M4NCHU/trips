import { FC } from "react";
import FormTitle from "./FormTitle";

interface FormSectionProps extends React.HTMLAttributes<HTMLDivElement> {
  title?: string;
  children: React.ReactNode;
}

const FormSection: FC<FormSectionProps> = ({ title, children, ...rest }) => {
  return (
    <div {...rest}>
      {title ? <FormTitle title={title} /> : ""}
      {children}
    </div>
  );
};

export default FormSection;
