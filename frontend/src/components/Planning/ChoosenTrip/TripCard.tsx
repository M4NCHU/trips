import { FC } from "react";
import { TbLogout2 } from "react-icons/tb";
import { Link } from "react-router-dom";
import UserTrigger from "src/components/Header/UserTrigger";
import CustomDropdownMenu from "src/components/ui/Dropdown/CustomDropdownMenu";
import CustomDropdownMenuItem from "src/components/ui/Dropdown/CustomDropdownMenuItem";
import { BsThreeDotsVertical, BsTrash } from "react-icons/bs";
import TripCardDropdownTrigger from "./TripCardDropdownTrigger";
import { Trip } from "src/types/TripTypes";

interface TripCardProps {
  data: Trip;
}

const TripCard: FC<TripCardProps> = ({ data }) => {
  return (
    <Link
      to={`/planning/${data.id}`}
      className="bg-secondary hover:opacity-90  p-4 rounded-lg flex justify-between items-center"
    >
      <div>
        <p className="font-semibold text-lg">{data.title}</p>
      </div>
      <div className="flex flex-row items-center gap-2">
        <CustomDropdownMenu dropDownButton={<TripCardDropdownTrigger />}>
          <CustomDropdownMenuItem label="Edit" href="/" />
          <CustomDropdownMenuItem label="Rename" href="/" />
          <div className="my-2 h-[1px] bg-gray-800"></div>
          <CustomDropdownMenuItem
            label="delete"
            variant="danger"
            onClick={() => {}}
            icon={<BsTrash />}
          />
        </CustomDropdownMenu>
      </div>
    </Link>
  );
};

export default TripCard;
