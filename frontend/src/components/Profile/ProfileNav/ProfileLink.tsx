import React, { FC } from "react";
import { Link } from "react-router-dom";

interface ProfileLinkInterface {
  link: string;
  title: string;
}

const ProfileLink: FC<ProfileLinkInterface> = ({ link, title }) => {
  return (
    <li className="profile-nav-item">
      <Link
        to={link}
        className="text-lg font-normal px-4 py-2 hover:bg-background rounded-lg"
      >
        {title}
      </Link>
    </li>
  );
};

export default ProfileLink;
