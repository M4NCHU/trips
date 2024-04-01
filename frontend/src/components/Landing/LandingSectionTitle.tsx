import { FC } from "react";
import { ButtonWithIcon } from "../ui/Buttons/ButtonWithIcon";
import { BsArrowLeft, BsArrowRight } from "react-icons/bs";
import { Link } from "react-router-dom";
import LandingSliderNavigation from "./LandingSliderNavigation";

interface LandingSectionTitleProps {
  title: string;
  desc: string;
  dataCount?: number;
}

const LandingSectionTitle: FC<LandingSectionTitleProps> = ({
  title,
  desc,
  dataCount,
}) => {
  return (
    <>
      <div className="flex justify-center">
        <div className="w-[12rem] h-[1px] bg-secondary"></div>
      </div>
      <div className="flex flex-row justify-between items-center gap-2">
        <div className="flex flex-col gap-1">
          <h2 className="text-base md:text-2xl font-semibold">{title}</h2>
          <p className="text-gray-400 text-sm md:text-base">{desc}</p>
        </div>
        <div className="flex flex-col md:flex-row gap-2 items-center text-sm md:text-base">
          <Link to="" className="px-1 md:px-4">
            View More
          </Link>
          {dataCount ? dataCount > 0 && <LandingSliderNavigation /> : ""}
        </div>
      </div>
    </>
  );
};

export default LandingSectionTitle;
