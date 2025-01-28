import { FC } from "react";

interface ItemFeaturesProps {
  description?: string;
  details?: string;
  cancellationPolicy?: string;
  additionalText?: string;
  features?: string[];
}

const CartItemFeatures: FC<ItemFeaturesProps> = ({
  description,
  details = "No details available",
  cancellationPolicy = "No cancellation policy",
  additionalText = "",
  features = [],
}) => {
  return (
    <div className="flex flex-col gap-4">
      {description && <p>{description}</p>}
      <div className="flex flex-col gap-1">
        <span>{details}</span>
        <p className="font-bold">{cancellationPolicy}</p>
        {additionalText && <p>{additionalText}</p>}
      </div>
      {features.length > 0 && (
        <ul className="item-features flex flex-row flex-wrap gap-2">
          {features.map((feature, index) => (
            <li key={index} className="bg-gray-100 px-2 py-1 rounded-md">
              {feature}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default CartItemFeatures;
