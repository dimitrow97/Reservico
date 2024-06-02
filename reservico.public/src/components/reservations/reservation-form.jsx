import { z } from "zod"
import { format } from "date-fns"
import { cn } from "@/lib/utils"
import {
    ClipboardPlus,
    CalendarIcon
} from "lucide-react"
import {
    Form,
    FormControl,
    FormDescription,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from "@/components/ui/form"
import {
    Popover,
    PopoverContent,
    PopoverTrigger,
} from "@/components/ui/popover"
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from "@/components/ui/select"
import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
    CardFooter
} from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Calendar } from "@/components/ui/calendar"
import { Input } from "@/components/ui/input"
import { useToast } from "@/components/ui/use-toast"
import { Textarea } from "@/components/ui/textarea"
import { ReservationSchema } from "../../schema"

import { useState } from 'react'
import { zodResolver } from "@hookform/resolvers/zod"
import { useForm } from "react-hook-form"
import { useMakeReservationMutation } from "@/features/reservations/reservations-api-slice"

const ReservationForm = ({ locationId, workingHoursFrom, workingHoursTo }) => {
    const [loading, setLoading] = useState(false)
    const { toast } = useToast()
    const [makeReservation] = useMakeReservationMutation()

    const form = useForm({
        resolver: zodResolver(ReservationSchema),
        defaultValues: {
            firstName: "",
            lastName: "",
            phoneNumber: "",
            email: "",
            note: "",
            date: "",
            hour: "",
            numberOfGuests: "1"
        },
    });

    const onSubmit = async (data) => {
        setLoading(true);

        data.date.setHours(data.hour)

        const reservationToMake = {
            firstName: data.firstName,
            lastName: data.lastName,
            phoneNumber: data.phoneNumber,
            email: data.email,
            note: data.note,
            guestsArrivingAt: data.date,
            numberOfGuests: data.numberOfGuests,
            locationId: locationId
        }

        try {
            const response = await makeReservation(reservationToMake).unwrap()

            if (response.isSuccess) {
                setLoading(false)
                toast({
                    title: "Reservation was booked Successfully!",
                    description: "You have successfully booked a Reservation! We have sent an email containing all the details.",
                })
            }
            else {
                toast({
                    title: "Booking the Reservation was unsuccessfull!"
                })
            }

            form.reset({
                firstName: "",
                lastName: "",
                phoneNumber: "",
                email: "",
                note: "",
                date: "",
                hour: "",
                numberOfGuests: "1"
            })

        } catch (err) {
            toast({
                title: "Booking the Reservation was unsuccessfull!"
            })
        }

        setLoading(false)
    };

    return (
        <Card x-chunk="dashboard-01-chunk-0">
            <CardHeader className="flex flex-row items-center justify-center space-y-0 pb-4 p-0 lg:p-4 xl:p-6">
                <CardTitle className="text-xl font-medium">
                    Book a Reservation
                </CardTitle>
            </CardHeader>
            <Form {...form}>
                <form onSubmit={form.handleSubmit(onSubmit)}>
                    <CardContent className="flex flex-col items-center justify-center text-center pb-4">
                        <div className="space-y-2 w-3/4 gap-2">
                            <div className="gap-2">
                                <FormField
                                    control={form.control}
                                    name="firstName"
                                    render={({ field }) => (
                                        <FormItem className="text-left pb-4">
                                            <FormLabel>First Name</FormLabel>
                                            <FormControl>
                                                <Input
                                                    {...field}
                                                    type="name"
                                                />
                                            </FormControl>
                                            <FormMessage />
                                        </FormItem>
                                    )}
                                />
                                <FormField
                                    control={form.control}
                                    name="lastName"
                                    render={({ field }) => (
                                        <FormItem className="text-left pb-4">
                                            <FormLabel>Last Name</FormLabel>
                                            <FormControl>
                                                <Input
                                                    {...field}
                                                    type="lastName"
                                                />
                                            </FormControl>
                                            <FormMessage />
                                        </FormItem>
                                    )}
                                />
                                <FormField
                                    control={form.control}
                                    name="email"
                                    render={({ field }) => (
                                        <FormItem className="text-left pb-4">
                                            <FormLabel>Email</FormLabel>
                                            <FormControl>
                                                <Input
                                                    {...field}
                                                    type="email"
                                                />
                                            </FormControl>
                                            <FormMessage />
                                        </FormItem>
                                    )}
                                />
                                <FormField
                                    control={form.control}
                                    name="phoneNumber"
                                    render={({ field }) => (
                                        <FormItem className="text-left pb-4">
                                            <FormLabel>Phone Number</FormLabel>
                                            <FormControl>
                                                <Input
                                                    {...field}
                                                    type="phoneNumber"
                                                />
                                            </FormControl>
                                            <FormMessage />
                                        </FormItem>
                                    )}
                                />
                                <FormField
                                    control={form.control}
                                    name="numberOfGuests"
                                    render={({ field }) => (
                                        <FormItem className="text-left pb-4">
                                            <FormLabel>Number of Guests</FormLabel>
                                            <FormControl>
                                                <Input
                                                    {...field}
                                                    type="number"
                                                />
                                            </FormControl>
                                            <FormMessage />
                                        </FormItem>
                                    )}
                                />
                                <FormField
                                    control={form.control}
                                    name="note"
                                    render={({ field }) => (
                                        <FormItem className="text-left pb-4">
                                            <FormLabel>Note</FormLabel>
                                            <FormControl>
                                                <Textarea
                                                    placeholder="Leave a note..."
                                                    className="resize-none"
                                                    {...field}
                                                />
                                            </FormControl>
                                            <FormDescription>
                                                You can <span>leave</span> a note with special requests or questions.
                                            </FormDescription>
                                            <FormMessage />
                                        </FormItem>
                                    )}
                                />
                                <FormField
                                    control={form.control}
                                    name="date"
                                    render={({ field }) => (
                                        <FormItem className="flex flex-col text-left pb-4">
                                            <FormLabel>Booking Date</FormLabel>
                                            <Popover>
                                                <PopoverTrigger asChild>
                                                    <FormControl>
                                                        <Button
                                                            variant={"outline"}
                                                            className={cn(
                                                                "pl-3 text-left font-normal",
                                                                !field.value && "text-muted-foreground"
                                                            )}
                                                        >
                                                            {field.value ? (
                                                                format(field.value, "PPP")
                                                            ) : (
                                                                <span>Pick a date</span>
                                                            )}
                                                            <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
                                                        </Button>
                                                    </FormControl>
                                                </PopoverTrigger>
                                                <PopoverContent className="w-auto p-0" align="start">
                                                    <Calendar
                                                        mode="single"
                                                        selected={field.value}
                                                        onSelect={field.onChange}
                                                        disabled={(date) =>
                                                            date < new Date("1900-01-01")
                                                        }
                                                        initialFocus
                                                    />
                                                </PopoverContent>
                                            </Popover>
                                            <FormDescription>
                                                Place the desired date for the Booking.
                                            </FormDescription>
                                            <FormMessage />
                                        </FormItem>
                                    )}
                                />
                                <FormField
                                    control={form.control}
                                    name="hour"
                                    render={({ field }) => (
                                        <FormItem className="text-left pb-4">
                                            <FormLabel>Hour of Arrival</FormLabel>
                                            <Select onValueChange={field.onChange} value={field.value}>
                                                <FormControl>
                                                    <SelectTrigger>
                                                        <SelectValue placeholder="Select a desired hour of arrival" />
                                                    </SelectTrigger>
                                                </FormControl>
                                                <SelectContent>
                                                    {Array.from({ length: workingHoursTo - workingHoursFrom + 1 }).map((_, index) => (
                                                        <SelectItem key={index} value={(index + workingHoursFrom).toString()}>{index + workingHoursFrom}:00</SelectItem>
                                                    ))}
                                                </SelectContent>
                                            </Select>
                                            <FormMessage />
                                        </FormItem>
                                    )}
                                />
                            </div>
                        </div>
                    </CardContent>
                    <CardFooter className="flex flex-col items-center justify-center">
                        <Button type="submit" className="w-1/2 bg-[#28282a] hover:border-2 hover:bg-white hover:text-black">
                            <ClipboardPlus className="mr-2 h-4 w-4" />
                            <span>{loading ? "Loading..." : "Book"}</span>
                        </Button>
                    </CardFooter>
                </form>
            </Form>
        </Card>
    )
}

export default ReservationForm