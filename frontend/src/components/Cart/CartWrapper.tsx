import { Children, FC } from "react";

interface CartWrapperProps {
  children: React.ReactNode;
}

const CartWrapper: FC<CartWrapperProps> = ({ children }) => {
  return (
    <div className="w-full flex flex-col lg:flex-row gap-4">{children}</div>
  );
};

export default CartWrapper;
