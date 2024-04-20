import { FC } from "react";
import SectionHeaderLayout from "./SectionHeaderLayout";
import SectionHeaderList from "./SectionHeaderList";

interface SectionHeaderProps {}

const SectionHeader: FC<SectionHeaderProps> = ({}) => {
  return (
    <SectionHeaderLayout>
      <SectionHeaderList />
    </SectionHeaderLayout>
  );
};

export default SectionHeader;
