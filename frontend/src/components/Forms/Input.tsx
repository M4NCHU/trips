import { FC, InputHTMLAttributes } from "react";
import FormLabel from "../Admin/Form/FormLabel";

interface InputProps extends InputHTMLAttributes<HTMLInputElement> {
  label?: string;
  errorMessage?: string;
}

const Input: FC<InputProps> = ({ label, errorMessage, ...inputProps }) => {
  return (
    <div className="flex flex-col gap-1">
      {label && <FormLabel title={label} />}
      <input className="p-3 rounded-lg bg-background" {...inputProps} />
      {errorMessage && (
        <div className="error-message text-red-500">{errorMessage}</div>
      )}
    </div>
  );
};

export default Input;
