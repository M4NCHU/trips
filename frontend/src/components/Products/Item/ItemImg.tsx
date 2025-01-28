import { FC } from "react";

interface ItemImgProps {
  imgSrc: string;
}

const ItemImg: FC<ItemImgProps> = ({ imgSrc }) => {
  return (
    <div className="h-[16rem] w-[16rem] overflow-hidden">
      <img
        src={imgSrc}
        alt="destination"
        className="w-full h-full object-cover rounded-lg"
      />
    </div>
  );
};

export default ItemImg;
