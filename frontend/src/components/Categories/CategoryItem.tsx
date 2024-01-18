import { FC } from "react";
import { Category } from "../../types/Category";

interface CategoryItemProps {
  data: Category;
}

const CategoryItem: FC<CategoryItemProps> = ({ data }) => {
  return (
    <div className="category-item flex flex-col items-center p-2 cursor-pointer border-b-2 border-transparent hover:border-black">
      <div className="category-item__img w-14 h-14 flex items-center justify-center">
        <img src={data.photoUrl} alt="" className="object-contain" />
      </div>
      <p>{data.name}</p>
    </div>
  );
};

export default CategoryItem;
