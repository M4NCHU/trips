import { FC } from "react";
import Landing from "src/pages/Landing/Landing";
import LandingCardItem from "./LandingCardItem";
import LandingItemsSection from "./LandingItemsSection";
import { UseCategoryList } from "src/api/Category";
import LandingSectionTitle from "./LandingSectionTitle";
import LandingItemsLayout from "./LandingItemsLayout";

interface LandingCategoriesListProps {}

const LandingCategoriesList: FC<LandingCategoriesListProps> = ({}) => {
  const { data: categories, isLoading, isError } = UseCategoryList();

  return (
    <LandingItemsLayout>
      <LandingSectionTitle
        title="Top Categories"
        desc="lorem ipsuasd fasd fas df asdfasdf"
      />
      <LandingItemsSection>
        {categories
          ? categories.map((item, i) => (
              <LandingCardItem key={i}>{item.name}</LandingCardItem>
            ))
          : "No categories"}
      </LandingItemsSection>
    </LandingItemsLayout>
  );
};

export default LandingCategoriesList;
