import { FC } from "react";

interface ItemActionsWrapperProps {
  children: React.ReactNode;
}

const ItemActionsWrapper: FC<ItemActionsWrapperProps> = ({ children }) => {
  return (
    <div className="flex flex-col xl:flex-row justify-between items-start xl:items-center">
      {children}
    </div>
  );
};

export default ItemActionsWrapper;
