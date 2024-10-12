import React, { useState, useEffect } from "react";
import * as FaIcons from "react-icons/fa";
import { IconType } from "react-icons";
import DynamicIcon from "./DynamicIcon";

const IconPicker: React.FC = () => {
  const [selectedIcon, setSelectedIcon] = useState<string | null>(null);
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [iconList, setIconList] = useState<string[]>([]);
  const [loadedIconsCount, setLoadedIconsCount] = useState(0);
  const [searchTerm, setSearchTerm] = useState("");
  const iconsPerPage = 40;

  const handleIconClick = (iconName: string) => {
    setSelectedIcon(iconName);
    setIsDropdownOpen(false);
  };

  const loadIconsInChunks = () => {
    setIsLoading(true);
    setTimeout(() => {
      const selectedIcons = Object.keys(FaIcons).filter((iconName) =>
        iconName.toLowerCase().includes(searchTerm.toLowerCase())
      );
      const nextIcons = selectedIcons.slice(
        loadedIconsCount,
        loadedIconsCount + iconsPerPage
      );
      setIconList((prev) => [...prev, ...nextIcons]);
      setLoadedIconsCount((prev) => prev + iconsPerPage);
      setIsLoading(false);
    }, 500);
  };

  useEffect(() => {
    if (isDropdownOpen && loadedIconsCount === 0) {
      loadIconsInChunks();
    }
  }, [isDropdownOpen]);

  useEffect(() => {
    setIconList([]);
    setLoadedIconsCount(0);
    if (isDropdownOpen) {
      loadIconsInChunks();
    }
  }, [searchTerm]);

  const handleScroll = (e: React.UIEvent<HTMLDivElement>) => {
    const { scrollTop, scrollHeight, clientHeight } = e.currentTarget;
    if (scrollHeight - scrollTop === clientHeight && !isLoading) {
      loadIconsInChunks();
    }
  };

  return (
    <div className="mt-8">
      <h2 className="text-xl font-bold mb-4">Pick an Icon for Category</h2>

      <div className="relative">
        <button
          type="button"
          onClick={() => setIsDropdownOpen(!isDropdownOpen)}
          className="w-full text-foreground font-semibold py-2 px-4 border bg-secondary rounded-lg shadow flex justify-between items-center"
        >
          {selectedIcon ? (
            <>
              {React.createElement(
                (FaIcons as { [key: string]: IconType })[selectedIcon],
                {
                  size: 24,
                  className: "text-blue-500",
                }
              )}
              <span className="ml-2">{selectedIcon}</span>
            </>
          ) : (
            <span>Select an Icon</span>
          )}
          <svg
            className={`w-5 h-5 ml-2 transition-transform duration-200 ${
              isDropdownOpen ? "rotate-180" : "rotate-0"
            }`}
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth="2"
              d="M19 9l-7 7-7-7"
            />
          </svg>
        </button>

        {isDropdownOpen && (
          <>
            <div
              className="absolute z-10 mt-2 w-full bg-secondary border text-foreground rounded-lg shadow-lg max-h-64 overflow-y-scroll"
              onScroll={handleScroll}
            >
              <div className="p-2 w-full">
                <input
                  type="text"
                  value={searchTerm}
                  onChange={(e) => setSearchTerm(e.target.value)}
                  placeholder="Search icons..."
                  className="p-3 w-full rounded-lg bg-background"
                />
              </div>
              <div className="grid grid-cols-6 gap-2 p-2">
                {iconList.map((iconName, i) => (
                  <DynamicIcon
                    key={i}
                    iconName={iconName as keyof typeof FaIcons}
                    onClick={handleIconClick}
                  />
                ))}
              </div>
              {isLoading && (
                <div className="w-full flex flex-row justify-center py-4">
                  <FaIcons.FaSpinner className="animate-spin h-5 w-5 mr-3 text-foreground" />
                </div>
              )}
            </div>
          </>
        )}
      </div>

      {selectedIcon && (
        <div className="mt-8">
          <h3 className="text-xl">Selected Icon: {selectedIcon}</h3>
          <div className="flex justify-center mt-4">
            {selectedIcon &&
              React.createElement(
                (FaIcons as { [key: string]: IconType })[selectedIcon],
                {
                  size: 64,
                  className: "text-blue-500",
                }
              )}
          </div>
        </div>
      )}
    </div>
  );
};

export default IconPicker;
