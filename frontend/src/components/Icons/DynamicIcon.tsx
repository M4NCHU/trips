import React, { FC } from "react";
import * as Icons from "react-icons/fa";

type IconName = keyof typeof Icons;

interface DynamicIconProps {
  iconName: IconName;
  onClick: (iconName: string) => void;
}

const DynamicIcon: FC<DynamicIconProps> = ({ iconName, onClick }) => {
  const IconComponent = Icons[iconName];
  return IconComponent ? (
    <div
      className="cursor-pointer flex justify-center items-center p-2 border rounded-lg hover:bg-background"
      onClick={() => onClick(iconName)}
    >
      <IconComponent size={24} className="text-foreground" />
    </div>
  ) : (
    <div>Ikona nie istnieje</div>
  );
};

export default DynamicIcon;
