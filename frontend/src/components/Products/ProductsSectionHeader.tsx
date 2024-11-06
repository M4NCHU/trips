import { FC } from "react";

interface ProductsSectionHeaderProps {
  title: string;
  children: React.ReactNode;
}

const ProductsSectionHeader: FC<ProductsSectionHeaderProps> = ({
  title,
  children,
}) => {
  return (
    <div className="flex flex-row items-center justify-between mb-[1.5rem]">
      <div className="flex flex-row items-center gap-4">
        <h1 className="text-2xl font-semibold">{title}</h1>
      </div>
      <div className="flex flex-row items-center gap-2">{children}</div>
    </div>
  );
};

export default ProductsSectionHeader;
