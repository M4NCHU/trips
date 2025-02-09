import { FC } from "react";

interface ItemFeaturesProps {}

const ItemFeatures: FC<ItemFeaturesProps> = ({}) => {
  return (
    <div className="flex flex-col gap-4">
      <div className="flex flex-col gap-1">
        <span>Organized trip</span>
      </div>

      <ul className="item-features flex flex-row flex-wrap gap-2">
        <li>Free parking</li>
        <li>Free internet</li>
      </ul>
    </div>
  );
};

export default ItemFeatures;
