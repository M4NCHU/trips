import { FC } from "react";
import { useDestinationList } from "../../api/Destinations";
import PaginationButtons from "../ui/PaginationButtons";
import DestinationItem from "./DestinationItem";
import DestinationsListSkeleton from "../Loading/Skeletons/DestinationsListSkeleton";

interface DestinationsListProps {}

const DestinationsList: FC<DestinationsListProps> = ({}) => {
  const {
    data: destinations,
    isError,
    page,
    isFetching,
  } = useDestinationList();

  return (
    <div className="flex flex-col items-center gap-12">
      <div className="destination-list flex flex-col md:flex-row flex-wrap gap-4 md:gap-2 mt-4 justify-center w-full">
        {destinations
          ? destinations.map((item, i) => (
              <DestinationItem key={i} data={item} />
            ))
          : "no data"}

        {isFetching ? (
          <DestinationsListSkeleton />
        ) : isError ? (
          <div>Error loading destinations</div>
        ) : isFetching ? (
          <DestinationsListSkeleton />
        ) : null}
      </div>
    </div>
  );
};

export default DestinationsList;
