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
  price: z
    .number()
    .min(0, "Price must be a non-negative number")
    .max(10000, "Price must be no more than 10000"),
  categoryId: z.string().regex(guidRegex, "categoryId must be a valid GUID"),
});

export type DestinationPayload = z.infer<typeof DestinationValidator>;
