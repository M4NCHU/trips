import { FC } from "react";

interface ItemDescriptionWrapperProps {
  children: React.ReactNode;
}

const ItemDescriptionWrapper: FC<ItemDescriptionWrapperProps> = ({
  children,
}) => {
  return (
    <div className="flex flex-col justify-between gap-2 p-2 grow">
      {children}
    </div>
  );
};

export default ItemDescriptionWrapper;
