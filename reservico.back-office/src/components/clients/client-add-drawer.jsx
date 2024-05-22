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
import { useToast } from "@/components/ui/use-toast"
import { useState } from 'react'
import { useAddClientMutation } from "../../features/clients/clients-api-slice"
import { ClientAddSchema } from "../../schema";

export function ClientAddDrawer() {
    const [loading, setLoading] = useState(false);
    const [open, setOpen] = useState(false);
    const { toast } = useToast()
    const [addClient] = useAddClientMutation()

    const form = useForm({
        resolver: zodResolver(ClientAddSchema),
        defaultValues: {
            name: "",
            address: "",
            city: "",
            postcode: "",
            country: ""
        },
    });

    const onSubmit = async (data) => {
        setLoading(true);

        const clientToAdd = {
            name: data.name,
            address: data.address,
            city: data.city,
            postCode: data.postcode,
            country: data.country,
        }

        try {
            const response = await addClient(clientToAdd).unwrap()

            console.log(response)

            if (response.isSuccess) {
                setLoading(false)
                toast({
                    title: "Client Created Successfully!",
                    description: "You have successfully create a new Client! ",
                })
            }
            else {
                toast({
                    title: "Creating the Client was unsuccessfull!",
                    description: response.errorMessage,
                })
            }

            setOpen(false);

        } catch (err) {
            console.log(err);
        }
    };

    return (
        <Drawer open={open} onOpenChange={setOpen}>
            <DrawerTrigger asChild>
                <Button variant="outline" className="ml-auto mr- shadow">Create a new Client</Button>
            </DrawerTrigger>
            <DrawerContent>
                <div className="mx-auto w-full max-w-sm">
                    <DrawerHeader>
                        <DrawerTitle>Create Client</DrawerTitle>
                    </DrawerHeader>
                    <Form {...form}>
                        <form onSubmit={form.handleSubmit(onSubmit)}>
                            <div className="space-y-2">
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
                                    name="address"
                                    render={({ field }) => (
                                        <FormItem className="text-left">
                                            <FormLabel>Address</FormLabel>
                                            <FormControl>
                                                <Input
                                                    {...field}
                                                    type="address"
                                                />
                                            </FormControl>
                                            <FormMessage />
                                        </FormItem>
                                    )}
                                />
                                <FormField
                                    control={form.control}
                                    name="city"
                                    render={({ field }) => (
                                        <FormItem className="text-left">
                                            <FormLabel>City</FormLabel>
                                            <FormControl>
                                                <Input
                                                    {...field}
                                                    type="city"
                                                />
                                            </FormControl>
                                            <FormMessage />
                                        </FormItem>
                                    )}
                                />
                                <FormField
                                    control={form.control}
                                    name="postcode"
                                    render={({ field }) => (
                                        <FormItem className="text-left">
                                            <FormLabel>Post Code</FormLabel>
                                            <FormControl>
                                                <Input
                                                    {...field}
                                                    type="postcode"
                                                />
                                            </FormControl>
                                            <FormMessage />
                                        </FormItem>
                                    )}
                                />
                                <FormField
                                    control={form.control}
                                    name="country"
                                    render={({ field }) => (
                                        <FormItem className="text-left">
                                            <FormLabel>Country</FormLabel>
                                            <FormControl>
                                                <Input
                                                    {...field}
                                                    type="country"
                                                />
                                            </FormControl>
                                            <FormMessage />
                                        </FormItem>
                                    )}
                                />
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

export default ClientAddDrawer