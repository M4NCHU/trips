import { FC, InputHTMLAttributes } from "react";
import { FiSearch } from "react-icons/fi";

interface ProductFilterInputProps
  extends InputHTMLAttributes<HTMLInputElement> {
  label: string;
  icon?: JSX.Element;
}

const ProductFilterInput: FC<ProductFilterInputProps> = ({
  label,
  icon = <FiSearch />,
  ...props
}) => {
  return (
    <div className="product-filter-input w-full px-1 mb-4">
      <label className="block text-gray-600 text-sm font-semibold mb-1">
        {label}
      </label>
      <div className="relative">
        {icon && (
          <span className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400">
            {icon}
          </span>
        )}
        <input
          {...props}
          placeholder={`Enter ${label.toLowerCase()}`}
          className="bg-background px-10 py-3 w-full rounded-lg text-primary placeholder-gray-400 
                     focus:outline-none focus:ring-2 focus:ring-indigo-500 transition duration-200"
        />
      </div>
    </div>
  );
};

export default ProductFilterInput;
