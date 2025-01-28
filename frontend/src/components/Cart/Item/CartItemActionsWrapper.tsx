import { FC } from "react";

interface ItemActionsWrapperProps {
  children: React.ReactNode;
}

const CartItemActionsWrapper: FC<ItemActionsWrapperProps> = ({ children }) => {
  return (
    <div className="flex flex-row justify-between items-center">{children}</div>
  );
};

export default CartItemActionsWrapper;
