import { FC } from "react";
import { CiTrash } from "react-icons/ci";
import { Button } from "src/components/ui/button";

interface ItemHeaderProps {
  title: string;
  description?: string;
  link?: string;
  onRemove?: () => void;
}

const CartItemHeader: FC<ItemHeaderProps> = ({
  title,
  description,
  link,
  onRemove,
}) => {
  return (
    <div className="item-desc flex flex-row justify-between">
      <div className="flex flex-col gap-2 mb-2">
        <h4 className="text-2xl font-semibold">
          {link ? <a href={link}>{title}</a> : title}
        </h4>
        {description && (
          <p className="text-foreground flex flex-row items-center gap-2 text-sm">
            {description}
          </p>
        )}
      </div>
      <div className="item-rating flex flex-col gap-2">
        {onRemove && (
          <Button className="bg-red-400" onClick={onRemove}>
            <CiTrash />
          </Button>
        )}
      </div>
    </div>
  );
};

export default CartItemHeader;
