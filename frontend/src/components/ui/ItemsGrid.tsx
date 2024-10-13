import { FC, ReactNode } from "react";

interface ItemsGridProps<T> {
  items: T[];
  renderItem: (item: T, index: number) => ReactNode;
}

const ItemsGrid: FC<ItemsGridProps<any>> = ({ items, renderItem }) => {
  return (
    <div className="">
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-2 lg:grid-cols-2 xl:grid-cols-2 2xl:grid-cols-3 3xl:grid-cols-4 4xl:grid-cols-4 5xl:grid-cols-4 gap-5">
        {items.map((item, index) => renderItem(item, index))}
      </div>
    </div>
  );
};

export default ItemsGrid;
