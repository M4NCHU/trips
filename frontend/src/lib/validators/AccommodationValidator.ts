import { z } from "zod";
import { guidRegex } from "./globalRegex";

export const AccommodationValidator = z.object({
  name: z
    .string()
    .min(3, "Name must be at least 3 characters long")
    .max(50, "Name must be no longer than 50 characters"),
  description: z
    .string()
    .min(3, "Description must be at least 3 characters long")
    .max(500, "Description must be no longer than 500 characters"),
  location: z
    .string()
    .min(3, "Location must be at least 3 characters long")
    .max(100, "Location must be no longer than 100 characters"),
  price: z.string().regex(/^\d+(\.\d{1,2})?$/, "Price must be a valid number"),
  bedAmount: z.string().regex(/^\d+$/, "Bed amount must be a valid integer"),
});

export type AccommodationPayload = z.infer<typeof AccommodationValidator>;
