import { FC } from "react";
import * as Icons from "react-icons/fa";

type IconName = keyof typeof Icons;

interface DynamicIconProps {
  iconName: IconName;
  iconClass?: string;
}

const DynamicIcon: FC<DynamicIconProps> = ({ iconName, iconClass }) => {
  const IconComponent = Icons[iconName];
  return IconComponent ? (
    <IconComponent className={`text-foreground ${iconClass}`} />
  ) : (
    <></>
  );
};

export default DynamicIcon;
