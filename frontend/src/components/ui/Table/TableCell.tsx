import { FC, ReactNode } from "react";

interface TableCellProps {
  className?: string;
  children: ReactNode;
}

const TableCell: FC<TableCellProps> = ({ className, children }) => {
  return <td className={className}>{children}</td>;
};

export default TableCell;
