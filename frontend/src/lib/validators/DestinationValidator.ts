import { z } from "zod";
import { guidRegex } from "./globalRegex";

export const DestinationValidator = z.object({
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
  price: z.string(),
  availablePlaces: z.string(),
  categoryId: z.string(),
});

export type DestinationPayload = z.infer<typeof DestinationValidator>;
