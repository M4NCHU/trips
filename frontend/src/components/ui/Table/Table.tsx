import { FC, useState } from "react";
import TableHeaderCell from "./TableHeaderCell";
import TableCell from "./TableCell";
import {
  FaSort,
  FaPlus,
  FaEdit,
  FaTrashAlt,
  FaFilter,
  FaSearch,
} from "react-icons/fa";
import { Button } from "../button";
import { IoCreate } from "react-icons/io5";
import { FiFilter } from "react-icons/fi";
import CreateDestinationModal from "src/components/Destinations/Modals/CreateDestinationModal";

interface TableProps<T> {
  data: T[];
  tableTitle?: string;
  tableDescription?: string;
  columns: (keyof T)[];
  renderCell?: (item: T, column: keyof T) => React.ReactNode;
  createModal?: React.ReactNode;
  tableClassName?: string;
  headerClassName?: string;
  rowClassName?: string;
  cellClassName?: string;
  onAdd?: () => void;
  onEdit?: (item: T) => void;
  onDelete?: (item: T) => void;
  onFilter?: () => void;
  onClearFilters?: () => void;
}

const Table = <T,>({
  data,
  tableTitle,
  tableDescription,
  columns,
  renderCell,
  createModal,
  tableClassName = "min-w-full bg-background rounded-xl",
  headerClassName = "px-6 py-3 text-left text-sm border-b-[1px] rounded-xl border-secondary font-bold",
  rowClassName = "even:bg-secondary hover:bg-secondary rounded-xl",
  cellClassName = "px-6 py-4 whitespace-nowrap hover:bg-secondary",
  onAdd,
  onEdit,
  onDelete,
  onFilter,
  onClearFilters,
}: TableProps<T>) => {
  const [sortColumn, setSortColumn] = useState<keyof T | null>(null);
  const [sortDirection, setSortDirection] = useState<"asc" | "desc">("asc");
  const [searchQuery, setSearchQuery] = useState("");

  // Styl przycisków
  const buttonStyle =
    "hidden md:block rounded-full text-base placeholder:text-sm w-full h-full px-[2rem] min-w-[15rem] py-[.5rem] pr-[2.5rem] bg-secondary";

  // Obsługa sortowania
  const handleSort = (column: keyof T) => {
    if (sortColumn === column) {
      setSortDirection(sortDirection === "asc" ? "desc" : "asc");
    } else {
      setSortColumn(column);
      setSortDirection("asc");
    }
  };

  const handleSearch = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearchQuery(e.target.value);
  };

  const filteredData = data.filter((item) =>
    columns.some((column) =>
      String(item[column]).toLowerCase().includes(searchQuery.toLowerCase())
    )
  );

  const sortedData = [...filteredData].sort((a, b) => {
    if (!sortColumn) return 0;
    const aValue = a[sortColumn];
    const bValue = b[sortColumn];
    if (typeof aValue === "string" && typeof bValue === "string") {
      return sortDirection === "asc"
        ? aValue.localeCompare(bValue)
        : bValue.localeCompare(aValue);
    }
    if (typeof aValue === "number" && typeof bValue === "number") {
      return sortDirection === "asc" ? aValue - bValue : bValue - aValue;
    }
    return 0;
  });

  return (
    <div className="table-container bg-background rounded-xl border-[1px] border-background">
      {/* Nagłówek tabeli z tytułem i opisem */}
      <div className="table-title-section flex flex-row items-center justify-between p-[1.2rem] border-b-[1px] border-secondary">
        <div className="table-title flex flex-col gap-3">
          <div className="table-title-header flex flex-row gap-[1.5rem] items-center">
            <h1 className="font-bold text-lg md:text-xl">
              {tableTitle && tableTitle}
            </h1>
            <span className="rounded-full text-sm md:text-normal bg-destructive text-red-500 px-3 py-1">
              {data.length} total
            </span>
          </div>
          <span className="text-sm md:text-normal">
            {tableDescription && tableDescription}
          </span>
        </div>
        <div className="table-actions flex flex-row gap-2">
          <Button className="bg-foreground-500 border-[2px] hover:bg-secondary transition-transform border-secondary text-foreground">
            <FiFilter />
            <span className="hidden md:block">Filter</span>
          </Button>
          {createModal && createModal}
        </div>
      </div>

      {/* Wyszukiwanie i filtry */}
      <div className="flex flex-wrap items-center justify-between p-[.5rem] border-b-[1px] border-secondary">
        <div className="flex items-center gap-4 flex-row overflow-x-auto">
          <div className="flex items-center rounded-md relative px-3 py-2">
            <FaSearch className="text-foreground mr-2 absolute right-[1.5rem]" />
            <input
              type="text"
              placeholder="Search Order"
              value={searchQuery}
              onChange={handleSearch}
              className={buttonStyle}
            />
          </div>

          <select className={buttonStyle}>
            <option value="Paid">Paid</option>
            <option value="Pending">Pending</option>
            <option value="Failed">Failed</option>
          </select>

          <select className={buttonStyle}>
            <option value="All Category">All Category</option>
            <option value="Category 1">Category 1</option>
            <option value="Category 2">Category 2</option>
          </select>

          {onFilter && (
            <button
              className="rounded-md px-4 py-2 flex items-center"
              onClick={onFilter}
            >
              <FaFilter className="mr-2" /> Filter
            </button>
          )}
        </div>

        <div className="flex gap-4">
          {onClearFilters && (
            <Button
              onClick={onClearFilters}
              className="bg-foreground-500 border-[2px] hover:bg-secondary transition-transform border-secondary text-foreground"
            >
              <span>Clear X</span>
            </Button>
          )}
          {onAdd && (
            <button
              className="bg-purple-600 text-foreground px-4 py-2 rounded-md flex items-center"
              onClick={onAdd}
            >
              <FaPlus className="mr-2" /> Add Customer
            </button>
          )}
        </div>
      </div>

      {/* Tabela */}
      <div className="overflow-x-auto">
        <table className={tableClassName}>
          <thead>
            <tr className="bg-background">
              {columns.map((column) => (
                <th key={String(column)} className={headerClassName}>
                  <button
                    onClick={() => handleSort(column)}
                    className="flex items-center"
                  >
                    {String(column).toUpperCase()}
                    <FaSort className="ml-2" />
                  </button>
                </th>
              ))}
              <th className={headerClassName}>Actions</th>
            </tr>
          </thead>
          <tbody>
            {sortedData.map((item, rowIndex) => (
              <tr key={rowIndex} className={rowClassName}>
                {columns.map((column) => (
                  <TableCell key={String(column)} className={cellClassName}>
                    {renderCell
                      ? renderCell(item, column)
                      : (item[column] as React.ReactNode)}
                  </TableCell>
                ))}
                <td className={cellClassName}>
                  {onEdit && (
                    <button
                      className="text-foreground py-1 px-3 rounded mr-2"
                      onClick={() => onEdit(item)}
                    >
                      <FaEdit />
                    </button>
                  )}
                  {onDelete && (
                    <button
                      className="text-white py-1 px-3 rounded"
                      onClick={() => onDelete(item)}
                    >
                      <FaTrashAlt />
                    </button>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <div className="table-footer flex p-[1.2rem] border-t-[1px] d-flex flex-row justify-between items-center w-full">
        <div></div>
        <div className="table-pagination flex flex-row items-center gap-2">
          <Button className="bg-foreground-500 border-[2px] hover:bg-secondary transition-transform border-secondary text-foreground">
            Previous
          </Button>
          <div className="flex gap-2">
            {/* Przykładowe numery stron */}
            <Button className="bg-foreground-500 hover:bg-secondary transition-transform text-foreground">
              1
            </Button>
            <Button className="bg-foreground-500 hover:bg-secondary transition-transform text-foreground">
              2
            </Button>
            <Button className="bg-foreground-500 hover:bg-secondary transition-transform text-foreground">
              3
            </Button>
          </div>
          <Button className="bg-foreground-500 border-[2px] hover:bg-secondary transition-transform border-secondary text-foreground">
            Next
          </Button>
        </div>
      </div>
    </div>
  );
};

export default Table;
