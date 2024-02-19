import React from "react";

const DestinationItemSkeleton = () => {
  return (
    <div className="category-item flex flex-col gap-4 p-2 cursor-pointer border-b-2 border-transparent hover:border-secondary animate-pulse">
      <div className="category-item__img flex items-center justify-center relative bg-gray-300 w-full md:w-72 h-72 rounded-xl"></div>
      <div className="destination-description flex flex-col gap-2">
        <div className="h-4 bg-gray-300 rounded w-3/4"></div>
        <div className="h-4 bg-gray-300 rounded w-1/2"></div>
        <div className="h-4 bg-gray-300 rounded w-1/4"></div>
      </div>
    </div>
  );
};

export default DestinationItemSkeleton;
