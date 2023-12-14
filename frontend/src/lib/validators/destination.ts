import { z } from "zod";

export const DestinationValidator = z.object({
  name: z.string(),
  description: z.string(),
  location: z.string(),
});

export type DestinationPayload = z.infer<typeof DestinationValidator>;
