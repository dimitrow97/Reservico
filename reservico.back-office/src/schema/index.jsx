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

export const CategoryAddSchema = z.object({
    name: z.string().min(2, {
        message: "Please enter a valid name"
    }),
})

export const UserAddSchema = z.object({
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
        message: "Please enter a valid Last name"
    }),
    clientId: z.string().min(36, {
        message: "Please enter a valid Client Id"
    }),
})

export const UserAddClientSchema = z.object({
    clientId: z.string().min(36, {
        message: "Please enter a valid Client Id"
    }),
})