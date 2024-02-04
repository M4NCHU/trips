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

    // Oczyść listener
    return () => {
      window.removeEventListener("click", pageClickEvent);
    };
  }, [isOpen]);

  const closeDropdown = () => {
    setIsOpen(false);
  };

  return (
    <div className="dropdown relative" ref={dropdownRef}>
      <button onClick={() => setIsOpen(!isOpen)}>{dropDownButton}</button>

      {isOpen && (
        <div className="absolute right-0 z-50 mt-2 min-w-[12rem] rounded-md shadow-lg bg-secondary p-2 ">
          <div className="flex flex-col rounded-lg" onClick={closeDropdown}>
            {children}
          </div>
        </div>
      )}
    </div>
  );
};

export default CustomDropdownMenu;
