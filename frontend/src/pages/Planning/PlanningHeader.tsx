import { FC } from "react";
import { Link } from "react-router-dom";

interface PlanningHeaderProps {}

const PlanningHeader: FC<PlanningHeaderProps> = ({}) => {
  return (
    <div className="bg-transparent border-secondary border-2 p-4 rounded-lg flex items-center justify-between mb-4">
      <div>
        <h1 className="font-bold text-xl">Choose trip scheme</h1>
      </div>
      <Link
        to={""}
        className="bg-green-600 hover:bg-green-500 flex flex-row gap-2 items-center px-4 py-2 rounded-lg text-white"
      >
        <p className="font-semibold">Create new trip scheme</p>
        <span className="font-bold text-2xl">+</span>
      </Link>
    </div>
  );
};

export default PlanningHeader;
