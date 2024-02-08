import { z } from "zod";

const minNameLength = 2;
const maxNameLength = 100;

export const VisitPlaceValidator = z.object({
  name: z
    .string()
    .min(
      minNameLength,
      `Name must be at least ${minNameLength} characters long`
    )
    .max(
      maxNameLength,
      `Name must be no longer than ${maxNameLength} characters`
    ),

  description: z
    .string()
    .min(
      minNameLength,
      `Description must be at least ${minNameLength} characters long`
    )
    .max(
      maxNameLength,
      `Description must be no longer than ${maxNameLength} characters`
    ),

  price: z.string(),

  destinationId: z.string(),
});

export type VisitPlacePayload = z.infer<typeof VisitPlaceValidator>;
