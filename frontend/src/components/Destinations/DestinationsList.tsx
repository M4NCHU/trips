import { FC } from "react";
import { GetDestinationList } from "../../api/Destinations";
import PaginationButtons from "../ui/PaginationButtons";
import DestinationItem from "./DestinationItem";

interface DestinationsListProps {}

const DestinationsList: FC<DestinationsListProps> = ({}) => {
  const {
    data: destinations,
    isPending,
    isError,
    fetchNextPage,
    fetchPreviousPage,
    page,
    isFetching,
  } = GetDestinationList();

  return (
    <div className="flex flex-col items-center gap-12">
      <div className="categories-list flex flex-row flex-wrap gap-4 md:gap-2 mt-4 justify-center ">
        {destinations
          ? destinations.map((item, i) => (
              <DestinationItem key={i} data={item} />
            ))
          : "no data"}

        {isPending ? (
          <div>Loading...</div>
        ) : isError ? (
          <div>Error loading destinations</div>
        ) : isFetching ? (
          <div>Fetching more destinations...</div>
        ) : null}
      </div>
      <PaginationButtons
        fetchPreviousPage={fetchPreviousPage}
        fetchNextPage={fetchNextPage}
        isFetching={isFetching}
        page={page}
      />
    </div>
  );
};

export default DestinationsList;
