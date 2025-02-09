import { FC } from "react";
import { CiLocationArrow1 } from "react-icons/ci";

interface ItemHeaderProps {
  title: string;
  description?: string;
}

const ItemHeader: FC<ItemHeaderProps> = ({ title, description }) => {
  return (
    <div className="item-desc flex flex-row justify-between">
      <div className=" flex flex-col gap-2 mb-2">
        <h4 className="text-2xl font-semibold">{title}</h4>
        <p className="text-foreground flex flex-row items-center gap-2 text-sm">
          {description ?? ""}
        </p>
      </div>
      <div className="item-rating flex flex-col gap-2">
        <p>Excellent</p>
        <span></span>
      </div>
    </div>
  );
};

export default ItemHeader;
