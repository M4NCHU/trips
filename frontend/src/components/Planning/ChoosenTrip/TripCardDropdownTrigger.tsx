import { FC } from "react";
import { BsThreeDotsVertical } from "react-icons/bs";

interface TripCardDropdownTriggerProps {}

const TripCardDropdownTrigger: FC<TripCardDropdownTriggerProps> = ({}) => {
  return (
    <div className="p-[.8rem] rounded-lg text-lg hover:bg-background">
      <BsThreeDotsVertical />
    </div>
  );
};

export default TripCardDropdownTrigger;
