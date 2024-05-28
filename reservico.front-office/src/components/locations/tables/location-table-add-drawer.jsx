import * as React from "react"
import {
    ClipboardPlus,
    CircleX
} from "lucide-react"
import { zodResolver } from "@hookform/resolvers/zod"
import { useForm } from "react-hook-form"
import { z } from "zod"
import { Button } from "@/components/ui/button"
import {
    Drawer,
    DrawerClose,
    DrawerContent,
    DrawerFooter,
    DrawerHeader,
    DrawerTitle,
    DrawerTrigger,
} from "@/components/ui/drawer"
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import { Checkbox } from "@/components/ui/checkbox"
import { useToast } from "@/components/ui/use-toast"
import { useState } from 'react'
import { useAddTableMutation } from "../../../features/locations/tables/tables-api-slice"
import { TableAddSchema } from "../../../schema";
import { apiSlice } from "../../../app/api/api-slice"
import { useDispatch, useSelector } from "react-redux"

export function LocationTableAddDrawer({ locationId }) {
    const [loading, setLoading] = useState(false)
    const [open, setOpen] = useState(false)
    const { toast } = useToast()
    const dispatch = useDispatch()
    const [addTable] = useAddTableMutation()

    const form = useForm({
        resolver: zodResolver(TableAddSchema),
        defaultValues: {
            name: "",
            capacity: "2",
            description: "",
            workingHoursFrom: "12",
            workingHoursTo: "22",
            canTableTurn: true,
            tableTurnOffset: "2"
        },
    });

    const onSubmit = async (data) => {
        setLoading(true);

        const tableToAdd = {
            name: data.name,
            capacity: data.capacity,
            description: data.description,
            workingHoursFrom: data.workingHoursFrom,
            workingHoursTo: data.workingHoursTo,
            canTableTurn: data.canTableTurn,
            tableTurnOffset: data.tableTurnOffset,
            locationId: locationId
        }

        try {
            const response = await addTable(tableToAdd).unwrap()

            if (response.isSuccess) {
                dispatch(apiSlice.util.invalidateTags(["tables"]))
                setLoading(false)
                toast({
                    title: "Table Created Successfully!",
                    description: "You have successfully create a new Tanle! ",
                })
            }
            else {
                toast({
                    title: "Creating the Table was unsuccessfull!",
                    description: response.errorMessage,
                })
            }

            setOpen(false);

            form.reset({
                name: "",
                capacity: "2",
                description: "",
                workingHoursFrom: "12",
                workingHoursTo: "22",
                canTableTurn: true,
                tableTurnOffset: "2"
            })

        } catch (err) {
            console.log(err);
        }
    };

    return (
        <Drawer open={open} onOpenChange={setOpen}>
            <DrawerTrigger asChild>
                <Button variant="outline" className="ml-auto mr- shadow">Create a new Table</Button>
            </DrawerTrigger>
            <DrawerContent>
                <div className="mx-auto w-full max-w-4xl">
                    <DrawerHeader>
                        <DrawerTitle>Create Table</DrawerTitle>
                    </DrawerHeader>
                    <Form {...form}>
                        <form onSubmit={form.handleSubmit(onSubmit)}>
                            <div className="space-y-2 grid grid-cols-2">
                                <div className="px-8">
                                    <FormField
                                        control={form.control}
                                        name="name"
                                        render={({ field }) => (
                                            <FormItem className="text-left">
                                                <FormLabel>Name</FormLabel>
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
                                        name="description"
                                        render={({ field }) => (
                                            <FormItem className="text-left">
                                                <FormLabel>Description</FormLabel>
                                                <FormControl>
                                                    <Input
                                                        {...field}
                                                        type="description"
                                                    />
                                                </FormControl>
                                                <FormMessage />
                                            </FormItem>
                                        )}
                                    />
                                    <FormField
                                        control={form.control}
                                        name="capacity"
                                        render={({ field }) => (
                                            <FormItem className="text-left">
                                                <FormLabel>Capacity</FormLabel>
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
                                        name="workingHoursFrom"
                                        render={({ field }) => (
                                            <FormItem className="text-left">
                                                <FormLabel>Working Hours From</FormLabel>
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
                                </div>
                                <div className="px-8">
                                    <FormField
                                        control={form.control}
                                        name="workingHoursTo"
                                        render={({ field }) => (
                                            <FormItem className="text-left">
                                                <FormLabel>Working Hours To</FormLabel>
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
                                        name="tableTurnOffset"
                                        render={({ field }) => (
                                            <FormItem className="text-left">
                                                <FormLabel>Table Turn Offset</FormLabel>
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
                                        name="canTableTurn"
                                        render={({ field }) => (
                                            <FormItem className="flex flex-row items-start space-x-3 space-y-0 rounded-md p-4">
                                                <FormControl>
                                                    <Checkbox
                                                        checked={field.value}
                                                        onCheckedChange={field.onChange}
                                                    />
                                                </FormControl>
                                                <div className="space-y-1 leading-none">
                                                    <FormLabel>Can the Table be Turned?</FormLabel>
                                                </div>
                                            </FormItem>
                                        )}
                                    />

                                </div>
                            </div>
                            <DrawerFooter>
                                <Button type="submit" className="bg-green-500 hover:bg-green-600">
                                    <ClipboardPlus className="mr-2 h-4 w-4" />
                                    <span>{loading ? "Loading..." : "Create"}</span>
                                </Button>
                                <DrawerClose asChild>
                                    <Button variant="outline">
                                        <CircleX className="mr-2 h-4 w-4" />
                                        <span>Cancel</span>
                                    </Button>
                                </DrawerClose>
                            </DrawerFooter>
                        </form>
                    </Form>
                </div>
            </DrawerContent>
        </Drawer>
    )
}

export default LocationTableAddDrawer