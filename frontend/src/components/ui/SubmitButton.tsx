import { FC, useEffect, useState } from "react";
import { FaSpinner, FaCheck } from "react-icons/fa";
import { Button } from "../../components/ui/button";

interface SubmitButtonProps {
  isPending: boolean;
  isSuccess: boolean;
  onSubmit: (e: any) => void;
}

const SubmitButton: FC<SubmitButtonProps> = ({
  isPending,
  isSuccess,
  onSubmit,
}) => {
  const [showCheckmark, setShowCheckmark] = useState(false);

  useEffect(() => {
    if (isSuccess && !showCheckmark) {
      setShowCheckmark(true);
      const timer = setTimeout(() => {
        setShowCheckmark(false);
      }, 2000);
      return () => clearTimeout(timer);
    }
  }, [isSuccess]);

  const handleClick = (e: any) => {
    if (!isPending && !isSuccess) {
      onSubmit(e);
    }
  };

  return (
    <Button
      className="mt-4 w-full bg-red-400"
      type="button"
      onClick={handleClick}
      disabled={isPending}
    >
      {isPending ? (
        <FaSpinner className="animate-spin" />
      ) : showCheckmark ? (
        <FaCheck className="text-green-500" />
      ) : (
        "Create +"
      )}
    </Button>
  );
};

export default SubmitButton;
