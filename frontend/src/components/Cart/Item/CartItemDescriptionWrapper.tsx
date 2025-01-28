import { FC } from "react";

interface ItemDescriptionWrapperProps {
  children: React.ReactNode;
}

const CartItemDescriptionWrapper: FC<ItemDescriptionWrapperProps> = ({
  children,
}) => {
  return (
    <div className="flex flex-col h-full justify-between gap-2 p-2 grow">
      {children}
    </div>
  );
};

export default CartItemDescriptionWrapper;
