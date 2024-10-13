import { FC } from "react";
import Slider from "react-slick";
import { UseCategoryList } from "src/api/Category";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import DynamicIcon from "src/components/Icons/DynamicIcon";
import * as Icons from "react-icons/fa";

type IconName = keyof typeof Icons;

interface Category {
  id: string;
  name: string;
  imageUrl: string;
}

const CategorySlider: FC = () => {
  const pageSize = 10;
  const filterParams = {};

  const {
    isLoading,
    isError,
    data: categories,
    totalItems,
    currentPage,
    page,
    setPage,
  } = UseCategoryList(filterParams, pageSize);

  const slidesToShow = Math.min(12, categories?.length || 1);

  const settings = {
    dots: true,
    infinite: false,
    speed: 500,
    slidesToShow: slidesToShow,
    slidesToScroll: 1,
    responsive: [
      {
        breakpoint: 1500,
        settings: {
          slidesToShow: Math.min(3, categories?.length || 1),
        },
      },
      {
        breakpoint: 768,
        settings: {
          slidesToShow: Math.min(2, categories?.length || 1),
        },
      },
      {
        breakpoint: 480,
        settings: {
          slidesToShow: Math.min(2, categories?.length || 1),
        },
      },
    ],
  };

  if (isLoading) return <div>Loading categories...</div>;
  if (isError) return <div>Failed to load categories</div>;

  return (
    <div className="mx-4 w-full overflow-hidden mt-4 px-12 pb-4">
      <Slider {...settings}>
        {categories?.map((category) => (
          <div
            key={category.id}
            className="hover:bg-background p-2 rounded-lg cursor-pointer"
          >
            <div className="category-slide flex flex-col justify-between grow  gap-4 items-center">
              <DynamicIcon
                iconName={category.icon as IconName}
                iconClass="w-8 h-8 text-blue-500"
              />

              <span className="text-base font-semibold text-nowrap text-center">
                {category.name}
              </span>
            </div>
          </div>
        ))}
      </Slider>
    </div>
  );
};

export default CategorySlider;
