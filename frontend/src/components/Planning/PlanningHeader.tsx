import { FC } from "react";

interface PlanningHeaderProps {
  title: string;
}

const PlanningHeader: FC<PlanningHeaderProps> = ({ title }) => {
  return (
    <div className="destination-header flex flex-row items-center gap-2">
      <h1 className="text-2xl font-bold">{title}</h1>
    </div>
  );
};

export default PlanningHeader;
