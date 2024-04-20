import { FC } from "react";
import SectionHeaderList from "./SectionHeaderList";

interface SectionHeaderLayoutProps {
  children: React.ReactNode;
}

const SectionHeaderLayout: FC<SectionHeaderLayoutProps> = ({ children }) => {
  return (
    <div className="flex flex-row p-2 bg-secondary  rounded-lg overflow-x-auto">
      {children}
    </div>
  );
};

export default SectionHeaderLayout;
