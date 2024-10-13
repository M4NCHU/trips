import React, { useState } from "react";
import { Button } from "src/components/ui/button";
import { useNavigate, useLocation } from "react-router-dom";

interface PaginationProps {
  totalPages: number;
  currentPage: number;
  onPageChange: (page: number) => void;
}

const Pagination: React.FC<PaginationProps> = ({
  totalPages,
  currentPage,
  onPageChange,
}) => {
  const navigate = useNavigate();
  const location = useLocation();
  const [inputPage, setInputPage] = useState<number | string>("");

  const changePage = (page: number) => {
    if (page >= 1 && page <= totalPages) {
      onPageChange(page);

      const searchParams = new URLSearchParams(location.search);
      searchParams.set("page", page.toString());
      navigate({ search: searchParams.toString() });
    }
  };

  const handleNext = () => {
    if (currentPage < totalPages) {
      changePage(currentPage + 1);
    }
  };

  const handlePrevious = () => {
    if (currentPage > 1) {
      changePage(currentPage - 1);
    }
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    if (value === "" || /^[0-9]+$/.test(value)) {
      setInputPage(value);
    }
  };

  const handleGoToPage = () => {
    const page = Number(inputPage);
    if (page >= 1 && page <= totalPages) {
      changePage(page);
      setInputPage("");
    }
  };

  const maxPageButtons = 10;
  const pagesToShow = Array.from(
    { length: Math.min(totalPages, maxPageButtons) },
    (_, index) => index + 1
  );

  return (
    <div className="table-pagination flex flex-row justify-end items-center gap-2">
      <Button
        className="bg-foreground-500 border-[2px] hover:bg-secondary transition-transform border-secondary text-foreground"
        onClick={handlePrevious}
        disabled={currentPage === 1}
      >
        Previous
      </Button>

      <div className="flex gap-2">
        {pagesToShow.map((page) => (
          <Button
            key={page}
            className={`${
              currentPage === page
                ? "bg-secondary text-foreground"
                : "bg-foreground-500 hover:bg-secondary transition-transform text-foreground"
            }`}
            onClick={() => changePage(page)}
          >
            {page}
          </Button>
        ))}

        {totalPages > maxPageButtons && (
          <div className="flex items-center">
            <input
              type="text"
              value={inputPage}
              onChange={handleInputChange}
              placeholder="Page"
              className="border bg-background text-foreground rounded px-2 w-16"
            />
            <Button
              className="ml-2 bg-foreground-500 border-[2px] hover:bg-secondary transition-transform border-secondary text-foreground"
              onClick={handleGoToPage}
              disabled={
                !inputPage ||
                Number(inputPage) > totalPages ||
                Number(inputPage) < 1
              }
            >
              Go
            </Button>
          </div>
        )}
      </div>

      <Button
        className="bg-foreground-500 border-[2px] hover:bg-secondary transition-transform border-secondary text-foreground"
        onClick={handleNext}
        disabled={currentPage === totalPages}
      >
        Next
      </Button>
    </div>
  );
};

export default Pagination;
