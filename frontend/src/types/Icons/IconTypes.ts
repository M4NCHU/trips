import * as Icons from "react-icons/fa";
export type IconName = keyof typeof Icons;
export function isValidIconName(iconName: string): iconName is IconName {
  return iconName in Icons;
}
