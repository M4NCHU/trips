import { FC } from "react";
import CategoryItem from "./CategoryItem";

interface CategoriesListProps {}

const CategoriesList: FC<CategoriesListProps> = ({}) => {
  return (
    <div className="categories-list flex flex-row container justify-center overflow-auto">
      <CategoryItem />
      <CategoryItem />
      <CategoryItem />
      <CategoryItem />
      <CategoryItem />
      <CategoryItem />
      <CategoryItem />
      <CategoryItem />
      <CategoryItem />
      <CategoryItem />
    </div>
  );
};

export default CategoriesList;
