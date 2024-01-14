import { FC } from "react";
import { Button } from "./button";

interface PaginationButtonsProps {
  fetchPreviousPage: () => void;
  fetchNextPage: () => void;
  isFetching: boolean;
  page: number;
}

const PaginationButtons: FC<PaginationButtonsProps> = ({
  fetchPreviousPage,
  fetchNextPage,
  isFetching,
  page,
}) => {
  return (
    <div className="pagination-buttons flex flex-row gap-2 items-center ">
      <Button
        className="bg-secondary hover:bg-background text-primary"
        onClick={fetchPreviousPage}
        disabled={isFetching || page === 1}
      >
        Prev
      </Button>
      <span>{page}</span>
      <Button
        className="bg-secondary text-primary"
        onClick={fetchNextPage}
        disabled={isFetching}
      >
        Next
      </Button>
    </div>
  );
};

export default PaginationButtons;
