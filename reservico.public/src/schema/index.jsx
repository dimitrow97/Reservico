import * as z from 'zod';

export const LoginSchema = z.object({
    email: z.string().email({
        message: "Please enter a valid email address"
    }),
    password: z.string().min(6, {
        message: "Password must be at least 6 characters long"
    })
})

export const LocationAddSchema = z.object({
    name: z.string().min(2, {
        message: "Please enter a valid Name"
    }),   
    address: z.string().min(2, {
        message: "Please enter a valid Address"
    }),
    city: z.string().min(2, {
        message: "Please enter a valid City"
    }),
    email: z.string().email({
        message: "Please enter a valid Email address"
    }),
    postcode: z.string().min(3, {
        message: "Please enter a valid Post code"
    }),
    country: z.string().min(3, {
        message: "Please enter a valid Country"
    })
})

export const TableAddSchema = z.object({
    name: z.string().min(2, {
        message: "Please enter a valid Name"
    }),       
    capacity: z.string().transform((v) => Number(v)||0),   
    description: z.string(),
    workingHoursFrom: z.string().transform((v) => Number(v)||0),
    workingHoursTo: z.string().transform((v) => Number(v)||0),
    canTableTurn: z.boolean(),
    tableTurnOffset: z.string().transform((v) => Number(v)||0),
})