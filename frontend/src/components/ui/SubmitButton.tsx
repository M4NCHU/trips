import React, { FC, useEffect, useState } from "react";
import { FaSpinner, FaCheck } from "react-icons/fa";
import { Button } from "../../components/ui/button"; // Upewnij się, że ścieżka do importu jest prawidłowa

interface SubmitButtonProps
  extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  isPending: boolean;
  isSuccess: boolean;
  isLoading?: boolean;
  onSubmit: (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
  variant?: "primary" | "secondary" | "success" | "danger";
}

const SubmitButton: FC<SubmitButtonProps> = ({
  isPending,
  isLoading,
  isSuccess,
  onSubmit,
  variant = "primary", // Domyślna wartość wariantu, jeśli nie zostanie podana
  ...props
}) => {
  const [showCheckmark, setShowCheckmark] = useState(false);

  useEffect(() => {
    if (isSuccess && !showCheckmark) {
      setShowCheckmark(true);
      const timer = setTimeout(() => setShowCheckmark(false), 2000);
      return () => clearTimeout(timer);
    }
  }, [isSuccess]);

  const handleClick = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    if (!isPending && !isSuccess) {
      onSubmit(e);
    }
  };

  // Mapowanie wariantów na klasy Tailwind CSS
  const variantClasses = {
    primary: "bg-blue-500 hover:bg-blue-600 text-white",
    secondary: "bg-gray-500 hover:bg-gray-600 text-white",
    success: "bg-green-500 hover:bg-green-600 text-white",
    danger: "bg-red-500 hover:bg-red-600 text-white",
  };

  return (
    <Button
      className={`mt-4 w-full ${variantClasses[variant]} hover:text-white disabled:bg-gray-400 disabled:cursor-not-allowed`}
      type="button"
      onClick={handleClick}
      disabled={isPending || isLoading}
      {...props}
    >
      {isPending ? (
        <FaSpinner className="animate-spin" />
      ) : showCheckmark ? (
        <FaCheck />
      ) : (
        props.children || "Submit"
      )}
    </Button>
  );
};

export default SubmitButton;
