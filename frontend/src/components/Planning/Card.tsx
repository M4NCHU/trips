import { FC } from "react";
import { BsThreeDotsVertical } from "react-icons/bs";

interface CardProps {
  content: React.ReactNode;
}

const Card: FC<CardProps> = ({ content }) => {
  return (
    <div className="flex flex-row gap-4 justify-between items-center w-full px-4 bg-secondary rounded-lg hover:opacity-90">
      <div className="flex flex-row gap-2 items-center">{content}</div>
      <div className="text-xl cursor-pointer p-2 flex items-center justify-center rounded-xl hover:bg-pink-600">
        <BsThreeDotsVertical />
      </div>
    </div>
  );
};

export default Card;
