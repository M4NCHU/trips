import { FC } from "react";

interface ItemImgProps {
  imgSrc: string;
}

const CartItemImg: FC<ItemImgProps> = ({ imgSrc }) => {
  return (
    <div className="h-[14rem] w-[14rem] overflow-hidden">
      <img
        src={imgSrc}
        alt="destination"
        className="w-full h-full object-cover rounded-lg"
      />
    </div>
  );
};

export default CartItemImg;
