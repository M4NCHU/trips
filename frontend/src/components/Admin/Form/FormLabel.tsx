import { FC } from "react";

interface FormLabelProps {
  title: string;
}

const FormLabel: FC<FormLabelProps> = ({ title }) => {
  return (
    <label className="font-bold py-1" htmlFor="">
      {title}
    </label>
  );
};

export default FormLabel;
