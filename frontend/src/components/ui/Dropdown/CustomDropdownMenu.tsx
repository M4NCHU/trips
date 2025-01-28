import React, { useState, useRef, useEffect } from "react";
import CustomDropdownMenuItem from "./CustomDropdownMenuItem";

interface CustomDropdownMenuProps {
  dropDownButton: React.ReactNode;
  children: React.ReactNode;
}

const CustomDropdownMenu = ({ dropDownButton, children }: any) => {
  const [isOpen, setIsOpen] = useState(false);
  const dropdownRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const pageClickEvent = (e: MouseEvent) => {
      if (
        dropdownRef.current !== null &&
        !dropdownRef.current.contains(e.target as Node)
      ) {
        setIsOpen(false);
      }
    };

    if (isOpen) {
      window.addEventListener("click", pageClickEvent);
    }

    return () => {
      window.removeEventListener("click", pageClickEvent);
    };
  }, [isOpen]);

  const closeDropdown = () => {
    setIsOpen(false);
  };

  const toggleDropdown = (event: React.MouseEvent<HTMLButtonElement>) => {
    event.preventDefault();
    setIsOpen(!isOpen);
  };

  return (
    <div className="dropdown relative" ref={dropdownRef}>
      <button onClick={toggleDropdown}>{dropDownButton}</button>

      {isOpen && (
        <div className="absolute right-0 z-50 min-w-[12rem] rounded-md shadow-lg bg-secondary p-2 border-[1px] border-gray-800 z-[450]">
          <div className="flex flex-col rounded-lg " onClick={closeDropdown}>
            {children}
          </div>
        </div>
      )}
    </div>
  );
};

export default CustomDropdownMenu;
