import { FC } from "react";

interface CartResumeSectionProps {
  children: React.ReactNode;
}

const CartResumeSection: FC<CartResumeSectionProps> = ({ children }) => {
  return <div className="lg:order-2 lg:w-1/3 ">{children}</div>;
};

export default CartResumeSection;
