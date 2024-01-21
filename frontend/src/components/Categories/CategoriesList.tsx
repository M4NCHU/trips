import { FC } from "react";
import CategoryItem from "./CategoryItem";
import { CiCirclePlus } from "react-icons/ci";
import { Link } from "react-router-dom";
import { UseCategoryList } from "../../api/Category";

interface CategoriesListProps {}

const CategoriesList: FC<CategoriesListProps> = ({}) => {
  const { data: categories, isLoading, isError } = UseCategoryList();

  return (
    <div className="categories-list flex flex-row container justify-center overflow-auto">
      {categories
        ? categories.map((category, i) => (
            <CategoryItem key={i} data={category} />
          ))
        : "No Categories"}

      <Link to={"/categories/create"}>
        <div className="category-item flex flex-col items-center p-2 cursor-pointer border-b-2 border-transparent hover:border-black">
          <div className="category-item__img w-14 h-14 flex items-center justify-center">
            <CiCirclePlus className="text-4xl" />
          </div>
          Create
        </div>
      </Link>
    </div>
  );
};

export default CategoriesList;
