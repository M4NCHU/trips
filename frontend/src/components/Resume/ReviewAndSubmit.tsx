import { FC } from "react";
import { CartItem } from "src/types/Cart/CartItem";
import CartItemDetails from "src/components/Cart/Item/CartItemDetails";
import CartItemImg from "../Cart/Item/CartItemImg";
import CartItemDescriptionWrapper from "../Cart/Item/CartItemDescriptionWrapper";
import CartItemHeader from "../Cart/Item/CartItemHeader";
import CartItemActionsWrapper from "../Cart/Item/CartItemActionsWrapper";
import CartItemFeatures from "../Cart/Item/CartItemFeatures";
import CartItemActions from "../Cart/Item/CartItemActions";
import { Button } from "../ui/button";

interface ReviewAndSubmitProps {
  formData: Record<string, string>;
  errors: Record<string, string>;
  cart: CartItem[];
  total: string;
  onRemove: (id: string) => void;
}

const ReviewAndSubmit: FC<ReviewAndSubmitProps> = ({
  formData,
  errors,
  cart,
  total,
  onRemove,
}) => {
  return (
    <div>
      <h2 className="text-xl font-bold mb-4">Review & Submit</h2>

      <div className="mb-6">
        <h3 className="text-lg font-bold">Personal Details</h3>
        <p>
          First Name:{" "}
          {formData.firstName || (
            <span className="text-red-500">Not provided</span>
          )}
        </p>
        <p>
          Last Name:{" "}
          {formData.lastName || (
            <span className="text-red-500">Not provided</span>
          )}
        </p>
        <p>
          Email:{" "}
          {formData.email || <span className="text-red-500">Not provided</span>}
        </p>
        <p>
          Phone Number:{" "}
          {formData.phoneNumber || (
            <span className="text-red-500">Not provided</span>
          )}
        </p>
        {Object.keys(errors).some((key) =>
          ["firstName", "lastName", "email", "phoneNumber"].includes(key)
        ) && (
          <p className="text-red-500">Please complete all personal details.</p>
        )}
      </div>

      <div className="mb-6">
        <h3 className="text-lg font-bold">Address Details</h3>
        <p>
          Address:{" "}
          {formData.address || (
            <span className="text-red-500">Not provided</span>
          )}
        </p>
        <p>
          City:{" "}
          {formData.city || <span className="text-red-500">Not provided</span>}
        </p>
        <p>
          Country:{" "}
          {formData.country || (
            <span className="text-red-500">Not provided</span>
          )}
        </p>
        <p>
          Postal Code:{" "}
          {formData.postalCode || (
            <span className="text-red-500">Not provided</span>
          )}
        </p>
        {Object.keys(errors).some((key) =>
          ["address", "city", "country", "postalCode"].includes(key)
        ) && (
          <p className="text-red-500">Please complete all address details.</p>
        )}
      </div>

      <div className="mb-6">
        <h3 className="text-lg font-bold">Payment Information</h3>
        <p>
          Card Number:{" "}
          {formData.cardNumber ? (
            `**** **** **** ${formData.cardNumber.slice(-4)}`
          ) : (
            <span className="text-red-500">Not provided</span>
          )}
        </p>
        <p>
          Expiration Date:{" "}
          {formData.expirationDate || (
            <span className="text-red-500">Not provided</span>
          )}
        </p>
        {Object.keys(errors).some((key) =>
          ["cardNumber", "expirationDate", "cvv"].includes(key)
        ) && (
          <p className="text-red-500">Please complete payment information.</p>
        )}
      </div>

      <div className="mb-6">
        <h3 className="text-lg font-bold">Cart Summary</h3>
        <p className="text-sm mb-4">
          Total: <span className="font-bold">{total} zł</span>
        </p>
        {cart.length ? (
          cart.map((item) => (
            <CartItemDetails
              key={item.id}
              item={item}
              onRemove={() => onRemove(item.id)}
            >
              <CartItemImg
                imgSrc={
                  item.destination?.photoUrl ||
                  "/images/default-destination.jpg"
                }
              />

              <CartItemDescriptionWrapper>
                <CartItemHeader
                  title={item.destination?.name || "Unknown Destination"}
                  link={`/destination/${item.itemId}`}
                  onRemove={() => onRemove(item.id)}
                />

                <CartItemActionsWrapper>
                  <CartItemFeatures
                    description={
                      item.destination?.description ||
                      "No description available"
                    }
                  />

                  <CartItemActions
                    price={item.destination?.price || 0}
                    quantity={item.quantity}
                    onRemove={() => onRemove(item.id)}
                  >
                    {item.quantity !== undefined && (
                      <p className="text-sm">
                        Quantity:{" "}
                        <span className="font-bold">{item.quantity}</span>
                      </p>
                    )}
                    {onRemove && (
                      <Button
                        onClick={() => onRemove}
                        className="text-red-500 hover:text-red-600"
                      >
                        Remove
                      </Button>
                    )}
                    <Button>Book now</Button>
                  </CartItemActions>
                </CartItemActionsWrapper>
              </CartItemDescriptionWrapper>
            </CartItemDetails>
          ))
        ) : (
          <p className="text-red-500">Your cart is empty.</p>
        )}
      </div>
    </div>
  );
};

export default ReviewAndSubmit;
