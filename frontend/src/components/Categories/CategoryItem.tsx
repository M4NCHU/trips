import { FC } from "react";
import Logo from "../../assets/images/logo.png";

interface CategoryItemProps {}

const CategoryItem: FC<CategoryItemProps> = ({}) => {
  return (
    <div className="category-item flex flex-col items-center p-2 cursor-pointer border-b-2 border-transparent hover:border-black">
      <div className="category-item__img w-14 h-14 flex items-center justify-center">
        <img src={Logo} alt="" className="object-contain" />
      </div>
      <p>title</p>
    </div>
  );
};

export default CategoryItem;
