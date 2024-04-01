import { FC } from "react";
import { ButtonWithIcon } from "../ui/Buttons/ButtonWithIcon";
import { BsArrowLeft } from "react-icons/bs";

interface LandingItemsSliderBtnProps {
  icon: React.ReactNode;
}

const LandingItemsSliderBtn: FC<LandingItemsSliderBtnProps> = ({ icon }) => {
  return <ButtonWithIcon icon={icon} className="p-1 w-[2rem] h-[2rem]" />;
};

export default LandingItemsSliderBtn;
