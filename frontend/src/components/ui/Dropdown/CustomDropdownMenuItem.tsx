import React, { ReactNode } from "react";
import { Link } from "react-router-dom";

interface CustomDropdownMenuItemProps {
  label: string;
  href?: string; // href jest teraz opcjonalne
  icon?: ReactNode;
  variant?: "default" | "primary" | "danger";
  className?: string;
  onClick?: () => void; // Opcjonalna akcja
}

const CustomDropdownMenuItem: React.FC<CustomDropdownMenuItemProps> = ({
  label,
  href,
  icon,
  variant = "default",
  className,
  onClick,
}) => {
  const variantClasses = {
    default: "hover:bg-background text-gray-400",
    primary: "hover:bg-blue-500 hover:text-white text-blue-700",
    danger: "hover:bg-red-400 hover:text-white bg-red-500",
  };

  const itemClasses = `p-2 w-full flex justify-start items-center gap-2 text-base rounded-lg ${variantClasses[variant]} ${className}`;

  // Renderowanie jako przycisk lub link w zależności od dostarczonych propsów
  return href ? (
    <Link className={itemClasses} to={href}>
      {icon && <span className="icon-container">{icon}</span>}
      <p className="dropdown-item">{label}</p>
    </Link>
  ) : (
    <button className={itemClasses} onClick={onClick}>
      {icon && <span className="icon-container">{icon}</span>}
      <p className="dropdown-item">{label}</p>
    </button>
  );
};

export default CustomDropdownMenuItem;
