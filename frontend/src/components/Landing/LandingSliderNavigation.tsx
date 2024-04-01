import { FC } from "react";
import LandingItemsSliderBtn from "./LandingItemsSliderBtn";
import { BsArrowLeft, BsArrowRight } from "react-icons/bs";

interface LandingSliderNavigationProps {}

const LandingSliderNavigation: FC<LandingSliderNavigationProps> = ({}) => {
  return (
    <div className="flex flex-row items-center gap-2">
      <LandingItemsSliderBtn icon={<BsArrowLeft />} />
      <LandingItemsSliderBtn icon={<BsArrowRight />} />
    </div>
  );
};

export default LandingSliderNavigation;
