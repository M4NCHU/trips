import { z } from "zod";

const minNameLength = 2;
const maxNameLength = 100;

export const CategoryValidator = z.object({
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
});

export type CategoryPayload = z.infer<typeof CategoryValidator>;
