import { FC } from "react";

interface LandingCardItemProps {
  children: React.ReactNode;
}

const LandingCardItem: FC<LandingCardItemProps> = ({ children }) => {
  return <div className="bg-secondary p-2 rounded-lg">{children}</div>;
};

export default LandingCardItem;
