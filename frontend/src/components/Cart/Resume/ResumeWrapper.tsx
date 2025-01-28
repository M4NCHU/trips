import { FC } from "react";

interface ResumeWrapperProps {
  children: React.ReactNode;
}

const ResumeWrapper: FC<ResumeWrapperProps> = ({ children }) => {
  return (
    <div className="flex flex-col gap-2 p-4 border rounded-lg bg-background">
      {children}
    </div>
  );
};

export default ResumeWrapper;
