import { FC, useState } from "react";
import toast from "react-hot-toast";
import { useAddToCart } from "src/api/Cart";
import SubmitButton from "src/components/ui/SubmitButton";
import { Destination } from "src/types/Destination";

interface ItemActionsProps {
  item: Destination;
}

const ItemActions: FC<ItemActionsProps> = ({ item }) => {
  const [loading, setLoading] = useState(false);
  const [showCheckmark, setShowCheckmark] = useState(false);

  const { mutate: addToCart, isPending, isSuccess } = useAddToCart();

  const handleAddToCart = (e: React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault();

    setLoading(true);
    addToCart(
      {
        userId: "",
        itemType: 1,
        itemId: item.id,
        quantity: 1,
        additionalData: item.description || "",
        createdAt: new Date().toISOString(),
        modifiedAt: new Date().toISOString(),
      },
      {
        onSuccess: () => {
          toast.success("Item added to cart successfully!");
          setShowCheckmark(true);
          setTimeout(() => setShowCheckmark(false), 2000);
        },
        onError: (error: any) => {
          console.error("Error adding to cart:", error);
          toast.error("Failed to add item to cart.");
        },
      }
    );
    setLoading(false);
  };

  return (
    <div className="flex flex-col gap-2">
      <div className="product-price flex flex-row items-center gap-2">
        <h3 className="font-bold text-2xl">{item.price} zł</h3>
        <span className="text-foreground text-base">/night</span>
      </div>
      <SubmitButton
        isPending={isPending}
        isSuccess={isSuccess}
        onSubmit={(e) => handleAddToCart(e)}
      >
        Book
      </SubmitButton>
    </div>
  );
};

export default ItemActions;
