import { FC } from "react";

interface LandingItemsSectionProps {
  children: React.ReactNode;
}

const LandingItemsSection: FC<LandingItemsSectionProps> = ({ children }) => {
  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 xl:grid-cols:5 gap-4">
      {children}
    </div>
  );
};

export default LandingItemsSection;
