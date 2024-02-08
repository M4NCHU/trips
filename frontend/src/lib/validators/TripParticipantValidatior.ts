import { z } from "zod";
import { guidRegex } from "./globalRegex";

export const ParticipantValidator = z.object({
  firstName: z
    .string()
    .min(1, "First name is required")
    .max(50, "First name cannot exceed 50 characters"),
  lastName: z
    .string()
    .min(1, "Last name is required")
    .max(50, "Last name cannot exceed 50 characters"),
  dateOfBirth: z.string().min(1, "Date of birth is required"),
  email: z
    .string()
    .email("Invalid email address")
    .max(100, "Email cannot exceed 100 characters"),
  phoneNumber: z
    .string()
    .min(1, "Phone number is required")
    .max(20, "Phone number cannot exceed 20 characters"),
  address: z
    .string()
    .min(1, "Address is required")
    .max(200, "Address cannot exceed 200 characters"),
  emergencyContact: z
    .string()
    .min(1, "Emergency contact name is required")
    .max(100, "Emergency contact name cannot exceed 100 characters"),
  emergencyContactPhone: z
    .string()
    .min(1, "Emergency contact phone is required")
    .max(20, "Emergency contact phone cannot exceed 20 characters"),
  medicalConditions: z
    .string()
    .max(500, "Medical conditions cannot exceed 500 characters")
    .optional(),
  createdAt: z.string(),
  tripId: z.string(),
});

export type ParticipantPayload = z.infer<typeof ParticipantValidator>;
