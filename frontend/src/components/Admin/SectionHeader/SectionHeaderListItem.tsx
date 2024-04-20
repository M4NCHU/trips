import { FC } from "react";
import { Link } from "react-router-dom";

interface SectionHeaderListItemProps {
  title: string;
  link: string;
}

const SectionHeaderListItem: FC<SectionHeaderListItemProps> = ({
  title,
  link,
}) => {
  return (
    <Link
      to={link}
      className="hover:text-primary text-base hover:font-semibold hover:bg-background px-4 py-2 rounded-lg"
    >
      {title}
    </Link>
  );
};

export default SectionHeaderListItem;
