import { FC, InputHTMLAttributes } from "react";

interface InputProps extends InputHTMLAttributes<HTMLInputElement> {
  label?: string;
}

const Input: FC<InputProps> = ({ label, ...inputProps }) => {
  return (
    <div className="flex flex-col gap-1">
      {label && (
        <label className="font-bold" htmlFor="">
          {label}
        </label>
      )}
      <input className="p-3 rounded-lg bg-background" {...inputProps} />
    </div>
  );
};

export default Input;
