import { FC, useEffect, useState } from "react";
import {
  useCart,
  useAddToCart,
  useRemoveFromCart,
  useDecreaseQuantity,
} from "src/api/Cart";
import CartWrapper from "src/components/Cart/CartWrapper";
import CartResumeSection from "src/components/Cart/CartResumeSection";
import CartItemsSection from "src/components/Cart/CartItemsSection";
import ProductsSectionHeader from "src/components/Products/ProductsSectionHeader";
import { Button } from "src/components/ui/button";
import { CartItem } from "src/types/Cart/CartItem";
import CartItemDetails from "src/components/Cart/Item/CartItemDetails";
import ResumeWrapper from "src/components/Cart/Resume/ResumeWrapper";
import ResumeHeader from "src/components/Cart/Resume/ResumeHeader";
import toast from "react-hot-toast";
import { Link } from "react-router-dom";
import CartItemImg from "src/components/Cart/Item/CartItemImg";
import CartItemDescriptionWrapper from "src/components/Cart/Item/CartItemDescriptionWrapper";
import CartItemHeader from "src/components/Cart/Item/CartItemHeader";
import CartItemActionsWrapper from "src/components/Cart/Item/CartItemActionsWrapper";
import CartItemFeatures from "src/components/Cart/Item/CartItemFeatures";
import CartItemActions from "src/components/Cart/Item/CartItemActions";

const Cart: FC = () => {
  const { data: apiCart, isError } = useCart();
  const { mutate: addToCart } = useAddToCart();
  const { mutate: removeFromCart } = useRemoveFromCart();
  const { mutate: decreaseQuantity } = useDecreaseQuantity();
  const [cart, setCart] = useState<CartItem[]>([]);

  useEffect(() => {
    if (apiCart && Array.isArray(apiCart)) {
      setCart(apiCart);
    } else if (isError) {
      console.error("Failed to load cart data from API.");
    }
  }, [apiCart, isError]);

  const handleAdd = (item: CartItem) => {
    addToCart(item, {
      onSuccess: () => {
        setCart((prevCart) =>
          prevCart.map((cartItem) =>
            cartItem.id === item.id
              ? { ...cartItem, quantity: cartItem.quantity + 1 }
              : cartItem
          )
        );
        toast.success("Quantity increased!");
      },
      onError: (error: any) => {
        console.error("Failed to increase quantity:", error);
      },
    });
  };

  const handleDecrease = (item: CartItem) => {
    decreaseQuantity(item, {
      onSuccess: () => {
        setCart((prevCart) =>
          prevCart
            .map((cartItem) =>
              cartItem.id === item.id
                ? {
                    ...cartItem,
                    quantity: Math.max(cartItem.quantity - 1, 0),
                  }
                : cartItem
            )
            .filter((cartItem) => cartItem.quantity > 0)
        );
        toast.success("Quantity decreased!");
      },
      onError: (error: any) => {
        console.error("Failed to decrease quantity:", error);
      },
    });
  };

  const handleRemove = (id: string) => {
    removeFromCart(id, {
      onSuccess: () => {
        setCart((prevCart) => prevCart.filter((item) => item.id !== id));
        toast.success("Successfully removed item from cart");
      },
      onError: (error: any) => {
        console.error("Failed to remove item from cart:", error);
      },
    });
  };

  const total = cart
    .reduce(
      (sum, item) => sum + (item.destination?.price || 0) * item.quantity,
      0
    )
    .toFixed(2);

  if (isError) return <p>Failed to load cart data. Please try again later.</p>;
  if (!cart.length) return <p>Your cart is empty.</p>;

  return (
    <CartWrapper>
      <CartResumeSection>
        <ResumeWrapper>
          <ResumeHeader title="Resume" />
          <p className="text-sm mt-2">
            Total: <span className="font-bold">{total} zł</span>
          </p>
          <Link
            to={"/resume"}
            className="mt-4 w-full px-4 py-2 text-white bg-green-500 rounded-md hover:bg-green-600"
          >
            Resume
          </Link>
        </ResumeWrapper>
      </CartResumeSection>

      <CartItemsSection>
        <ProductsSectionHeader title="Your Cart" />
        {cart.map((item) => (
          <CartItemDetails
            key={item.id}
            item={item}
            onRemove={() => handleRemove(item.id)}
          >
            <CartItemImg
              imgSrc={
                item.destination?.photoUrl || "/images/default-destination.jpg"
              }
            />

            <CartItemDescriptionWrapper>
              <CartItemHeader
                title={item.destination?.name || "Unknown Destination"}
                link={`/destination/${item.itemId}`}
                onRemove={() => handleRemove(item.id)}
              />

              <CartItemActionsWrapper>
                <CartItemFeatures
                  description={
                    item.destination?.description || "No description available"
                  }
                />

                <CartItemActions
                  price={item.destination?.price || 0}
                  quantity={item.quantity}
                  onRemove={() => handleRemove(item.id)}
                >
                  <p className="text-sm">
                    Quantity: <span className="font-bold">{item.quantity}</span>
                  </p>
                  <div className="flex gap-2 mt-2">
                    <Button
                      onClick={() => handleDecrease(item)}
                      className="bg-red-500 text-white hover:bg-red-600 px-2 py-1 rounded-md"
                    >
                      -
                    </Button>
                    <Button
                      onClick={() => handleAdd(item)}
                      className="bg-blue-500 text-white hover:bg-blue-600 px-2 py-1 rounded-md"
                    >
                      +
                    </Button>
                  </div>
                </CartItemActions>
              </CartItemActionsWrapper>
            </CartItemDescriptionWrapper>
          </CartItemDetails>
        ))}
      </CartItemsSection>
    </CartWrapper>
  );
};

export default Cart;
