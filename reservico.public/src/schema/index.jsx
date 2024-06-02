import * as z from 'zod';

export const ReservationSchema = z.object({
    date: z.date({
        required_error: "A date for the reservation is required.",
    }),
    email: z.string().email({
        message: "Please enter a valid email address"
    }),
    firstName: z.string().min(2, {
        message: "Please enter a valid First name"
    }),
    lastName: z.string().min(2, {
        message: "Please enter a valid Last name"
    }),
    phoneNumber: z.string().min(10, {
        message: "Please enter a valid Phone number"
    }),
    note: z.string(),
    hour: z.string(),
    numberOfGuests: z.string().transform((v) => Number(v)||0)
})