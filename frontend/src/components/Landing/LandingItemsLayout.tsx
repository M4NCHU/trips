import { FC } from "react";

interface LandingItemsLayoutProps {
  children: React.ReactNode;
}

const LandingItemsLayout: FC<LandingItemsLayoutProps> = ({ children }) => {
  return <div className="flex flex-col gap-4">{children}</div>;
};

export default LandingItemsLayout;
