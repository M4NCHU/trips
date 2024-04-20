import { Children, FC } from "react";
import SectionHeaderListItem from "./SectionHeaderListItem";

interface SectionHeaderListProps {}

const SectionHeaderList: FC<SectionHeaderListProps> = ({}) => {
  return (
    <ul className="flex flex-row  whitespace-nowrap ">
      {" "}
      <SectionHeaderListItem title="All destinations" link="/" />
      <SectionHeaderListItem title="Create +" link="/" />
    </ul>
  );
};

export default SectionHeaderList;
