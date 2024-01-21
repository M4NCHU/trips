import { FC } from "react";
import { Link } from "react-router-dom";

interface CardTitleProps {
  image: string;
  alt: string;
  title: string;
  link?: string;
}

const CardTitle: FC<CardTitleProps> = ({ image, alt, title, link }) => {
  return (
    <div className=" flex flex-row gap-2 items-center">
      <div className="relative w-16 h-16 flex items-center">
        <img src={image} alt={alt} className="object-cover rounded-xl" />
      </div>
      {link ? <Link to={link}>{title}</Link> : <p>{title}</p>}
    </div>
  );
};

export default CardTitle;
