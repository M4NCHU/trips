import { z } from "zod";

export const ResumeValidator = z.object({
  firstName: z
    .string()
    .min(2, "First Name must be at least 2 characters long")
    .max(100, "First Name must be no longer than 100 characters"),
  lastName: z
    .string()
    .min(2, "Last Name must be at least 2 characters long")
    .max(100, "Last Name must be no longer than 100 characters"),
  email: z.string().email("Invalid email address"),
  phoneNumber: z.string(),

  address: z
    .string()
    .min(5, "Address must be at least 5 characters long")
    .max(500, "Address must be no longer than 500 characters"),
  city: z
    .string()
    .min(2, "City must be at least 2 characters long")
    .max(100, "City must be no longer than 100 characters"),
  country: z
    .string()
    .min(2, "Country must be at least 2 characters long")
    .max(100, "Country must be no longer than 100 characters"),
  postalCode: z.string().regex(/^[0-9]{5,10}$/, "Invalid postal code"),

  cardNumber: z
    .string()
    .regex(/^\d{16}$/, "Card number must be exactly 16 digits"),
  expirationDate: z
    .string()
    .regex(
      /^(0[1-9]|1[0-2])\/\d{2}$/,
      "Expiration date must be in MM/YY format"
    ),
  cvv: z.string().regex(/^\d{3,4}$/, "CVV must be 3 or 4 digits"),
});

export type ResumePayload = z.infer<typeof ResumeValidator>;
