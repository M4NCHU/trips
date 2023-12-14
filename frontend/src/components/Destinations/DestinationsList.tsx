import { FC } from "react";
import DestinationItem from "./DestinationItem";

interface DestinationsListProps {}

const DestinationsList: FC<DestinationsListProps> = ({}) => {
  return (
    <div className="categories-list flex flex-row container flex-wrap mt-4 justify-center ">
      <DestinationItem />
      <DestinationItem />
      <DestinationItem />
      <DestinationItem />
      <DestinationItem />
      <DestinationItem />
      <DestinationItem />
      <DestinationItem />
      <DestinationItem />
      <DestinationItem />
    </div>
  );
};

export default DestinationsList;
