import { FC } from "react";

interface TableHeaderCellProps {
  className?: string;
  column: string;
}

const TableHeaderCell: FC<TableHeaderCellProps> = ({ className, column }) => {
  return <th className={className}>{column.toUpperCase()}</th>;
};

export default TableHeaderCell;
