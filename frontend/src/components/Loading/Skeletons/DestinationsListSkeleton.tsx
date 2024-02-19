import React from "react";
import DestinationItemSkeleton from "./DestinationItemSkeleton";

const DestinationsListSkeleton = () => {
  // Przykład generowania 10 skeletonów dla elementów listy
  const skeletons = Array(10).fill(0);

  return (
    <div className="flex flex-col items-center gap-12">
      <div className="categories-list flex flex-row flex-wrap gap-4 md:gap-2 mt-4 justify-center">
        {skeletons.map((_, i) => (
          <DestinationItemSkeleton key={i} />
        ))}
      </div>
    </div>
  );
};

export default DestinationsListSkeleton;
