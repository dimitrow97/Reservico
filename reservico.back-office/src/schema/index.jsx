import * as z from 'zod';

export const LoginSchema = z.object({
    email: z.string().email({
        message: "Please enter a valid email address"
    }),
    password: z.string().min(6, {
        message: "Password must be at least 6 characters long"
    })
})

export const ClientAddSchema = z.object({
    name: z.string().min(3, {
        message: "Please enter a valid name"
    }),
    address: z.string(),
    city: z.string(),
    postcode: z.string(),
    country: z.string()
})