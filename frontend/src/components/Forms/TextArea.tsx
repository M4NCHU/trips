import { FC, TextareaHTMLAttributes } from "react";

interface TextAreaProps extends TextareaHTMLAttributes<HTMLTextAreaElement> {
  label?: string;
}

const TextArea: FC<TextAreaProps> = ({ label, ...textareaProps }) => {
  return (
    <div className="flex flex-col gap-1">
      {label && (
        <label className="font-bold" htmlFor="">
          {label}
        </label>
      )}
      <textarea className="p-3 rounded-lg bg-background" {...textareaProps} />
    </div>
  );
};

export default TextArea;
