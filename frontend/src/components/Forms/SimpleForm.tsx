import React, { FC } from "react";

interface FormField {
  name: string;
  label: string;
  type: string;
  placeholder?: string;
}

interface DynamicFormProps {
  fields: FormField[];
  formData: Record<string, string>;
  errors: Record<string, string>;
  handleInputChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
}

const SimpleForm: FC<DynamicFormProps> = ({
  fields,
  formData,
  errors,
  handleInputChange,
}) => {
  return (
    <div>
      <h2 className="text-xl font-bold mb-4">Dynamic Form</h2>
      <form className="bg-background px-2 py-4 rounded-lg flex flex-col gap-2">
        {fields.map((field) => (
          <div key={field.name} className="mb-4 flex flex-col gap-2">
            <label className="block text-sm font-medium" htmlFor={field.name}>
              {field.label}
            </label>
            <input
              id={field.name}
              type={field.type}
              name={field.name}
              value={formData[field.name] || ""}
              onChange={handleInputChange}
              className={`w-full border bg-secondary rounded-md p-2 ${
                errors[field.name] ? "border-red-500" : ""
              }`}
              placeholder={field.placeholder || ""}
            />
            {errors[field.name] && (
              <p className="text-red-500 text-sm mt-1">{errors[field.name]}</p>
            )}
          </div>
        ))}
      </form>
    </div>
  );
};

export default SimpleForm;
