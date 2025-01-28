import { Children, FC } from "react";
import CartItemImg from "src/components/Cart/Item/CartItemImg";
import CartItemDescriptionWrapper from "src/components/Cart/Item/CartItemDescriptionWrapper";
import CartItemHeader from "src/components/Cart/Item/CartItemHeader";
import CartItemActionsWrapper from "src/components/Cart/Item/CartItemActionsWrapper";
import CartItemFeatures from "src/components/Cart/Item/CartItemFeatures";
import CartItemActions from "src/components/Cart/Item/CartItemActions";
import { CartItem } from "src/types/Cart/CartItem";

interface CartItemDetailsProps {
  item?: CartItem;
  onRemove?: (id: string) => void;
  children: React.ReactNode;
}

const CartItemDetails: FC<CartItemDetailsProps> = ({
  item,
  onRemove,
  children,
}) => {
  return (
    <div className="flex items-center mb-4 border-b pb-4 bg-background p-4 rounded-lg gap-4">
      {children}
    </div>
  );
};

export default CartItemDetails;
