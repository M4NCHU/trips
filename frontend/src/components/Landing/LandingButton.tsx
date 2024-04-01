import { FC } from "react";
import { Link } from "react-router-dom";

interface LandingButtonProps {
  text: string;
  link: string;
}

const LandingButton: FC<LandingButtonProps> = ({ text, link, ...props }) => {
  return (
    <Link
      to={link}
      {...props}
      className="px-4 py-2 text-sm md:text-base bg-pink-600 hover:bg-secondary transition-all duration-100  rounded-full"
    >
      {text}
    </Link>
  );
};

export default LandingButton;
