import { FC } from "react";
import { Button } from "src/components/ui/button";

interface ItemActionsProps {
  price: number;
  quantity?: number;
  onRemove?: () => void;
  children: React.ReactNode;
}

const CartItemActions: FC<ItemActionsProps> = ({
  price,
  quantity,
  onRemove,
  children,
}) => {
  return (
    <div className="flex flex-col gap-2">
      <div className="product-price flex flex-row items-center gap-2">
        <h3 className="font-bold text-2xl">{price} zł</h3>
        <span className="text-foreground text-base">/night</span>
      </div>
      {children}
    </div>
  );
};

export default CartItemActions;
