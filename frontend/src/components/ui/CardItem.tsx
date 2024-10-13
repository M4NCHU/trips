import { FC, ReactNode } from "react";

interface CardItemProps {
  imageUrl: string;
  title: string;
  description?: string;
  price?: string;
  actions?: ReactNode;
}

const CardItem: FC<CardItemProps> = ({
  imageUrl,
  title,
  description,
  price,
  actions,
}) => {
  return (
    <div className="flex flex-col gap-2 p-4 bg-transparent rounded-lg ">
      <div className="w-full h-[16rem] overflow-hidden">
        <img
          src={imageUrl}
          alt={title}
          className="w-full h-full object-cover rounded-lg"
        />
      </div>
      <div className="flex flex-col gap-2">
        <h4 className="text-lg font-semibold">{title}</h4>
        {description && <p className="text-gray-500 text-sm">{description}</p>}
        {price && <span className="font-bold text-lg">{price}</span>}
        {actions && <div className="flex justify-end mt-2">{actions}</div>}
      </div>
    </div>
  );
};

export default CardItem;
