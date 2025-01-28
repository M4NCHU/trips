import { Children, FC } from "react";

interface CartItemsSectionProps {
  children: React.ReactNode;
}

const CartItemsSection: FC<CartItemsSectionProps> = ({ children }) => {
  return <div className="lg:w-2/3 space-y-4">{children}</div>;
};

export default CartItemsSection;
