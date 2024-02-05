import { z } from "zod";

const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
const passwordStrengthRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$/;
const minNameLength = 2;
const maxNameLength = 100;

const minEmailLength = 5;
const maxEmailLength = 100;

const minPasswordLength = 6;
const maxPasswordLength = 50;

// Define a reusable email field validator
const emailFieldValidator = z
  .string()
  .min(
    minEmailLength,
    `Email must be at least ${minEmailLength} characters long`
  )
  .max(
    maxEmailLength,
    `Email must be no longer than ${maxEmailLength} characters`
  )
  .regex(emailRegex, "Invalid email format");

export const UserLoginValidator = z.object({
  username: z
    .string()
    .min(
      minNameLength,
      `Name must be at least ${minNameLength} characters long`
    )
    .max(
      maxNameLength,
      `Name must be no longer than ${maxNameLength} characters`
    ),
  password: z
    .string()
    .min(
      minPasswordLength,
      `Password must be at least ${minPasswordLength} characters long`
    )
    .max(
      maxPasswordLength,
      `Password must be no longer than ${maxPasswordLength} characters`
    ),
});

export const UserRegistrationValidator = z.object({
  firstName: z
    .string()
    .min(
      minNameLength,
      `First name must be at least ${minNameLength} characters long`
    )
    .max(
      maxNameLength,
      `First name must be no longer than ${maxNameLength} characters`
    ),
  lastName: z
    .string()
    .min(
      minNameLength,
      `Last name must be at least ${minNameLength} characters long`
    )
    .max(
      maxNameLength,
      `Last name must be no longer than ${maxNameLength} characters`
    ),
  username: z
    .string()
    .min(
      minNameLength,
      `Name must be at least ${minNameLength} characters long`
    )
    .max(
      maxNameLength,
      `Name must be no longer than ${maxNameLength} characters`
    ),
  email: emailFieldValidator,
  password: z
    .string()
    .min(
      minPasswordLength,
      `Password must be at least ${minPasswordLength} characters long`
    )
    .max(
      maxPasswordLength,
      `Password must be no longer than ${maxPasswordLength} characters`
    )
    .regex(
      passwordStrengthRegex,
      "Password must include at least one number, one uppercase letter, one lowercase letter, and one special character"
    ),
});

export type UserRegistrationPayload = z.infer<typeof UserRegistrationValidator>;
export type UserLoginPayload = z.infer<typeof UserLoginValidator>;
