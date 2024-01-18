import { FC } from "react";

interface CardTitleProps {
  image: string;
  alt: string;
  title: string;
}

const CardTitle: FC<CardTitleProps> = ({ image, alt, title }) => {
  return (
    <div className=" flex flex-row gap-2 items-center">
      <div className="relative w-16 h-16 flex items-center">
        <img src={image} alt={alt} className="object-cover rounded-xl" />
      </div>
      <p>{title}</p>
    </div>
  );
};

export default CardTitle;
