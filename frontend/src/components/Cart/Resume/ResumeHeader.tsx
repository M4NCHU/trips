import { FC } from "react";

interface ResumeHeaderProps {
  title: string;
}

const ResumeHeader: FC<ResumeHeaderProps> = ({ title }) => {
  return <h3 className="text-lg font-semibold ">{title}</h3>;
};

export default ResumeHeader;
