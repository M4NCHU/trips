import { FC } from "react";

interface StepperNavigationProps {
  steps: { id: number; label: string }[];
  currentStep: number;
}

const StepperNavigation: FC<StepperNavigationProps> = ({
  steps,
  currentStep,
}) => {
  return (
    <div className="flex justify-center items-center gap-4 mb-6">
      {steps.map((step) => (
        <div
          key={step.id}
          className={`flex items-center gap-2 ${
            currentStep === step.id
              ? "font-bold text-blue-500"
              : "text-gray-400"
          }`}
        >
          <span className="w-8 h-8 flex justify-center items-center rounded-full border border-current">
            {step.id}
          </span>
          <span>{step.label}</span>
        </div>
      ))}
    </div>
  );
};

export default StepperNavigation;
