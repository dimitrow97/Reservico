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
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from "@/components/ui/select"
import { Input } from "@/components/ui/input"
import { useToast } from "@/components/ui/use-toast"
import { useState } from 'react'
import { useAddUserMutation } from "../../features/users/users-api-slice"
import { useGetClientsQuery } from "../../features/clients/clients-api-slice"
import { UserAddSchema } from "../../schema";
import { apiSlice } from "../../app/api/api-slice"
import { useDispatch } from "react-redux"
import Loader from "../common/loader"

export function UserAddDrawer() {
    const {
        data: clients,
        isLoading,
        isSuccess,
        isError,
        error
    } = useGetClientsQuery()

    const [loading, setLoading] = useState(false);
    const [open, setOpen] = useState(false);
    const { toast } = useToast()
    const dispatch = useDispatch()
    const [addUser] = useAddUserMutation()

    const form = useForm({
        resolver: zodResolver(UserAddSchema),
        defaultValues: {
            email: "",
            firstName: "",
            lastName: "",
            phoneNumber: "",
            clientId: "",
        },
    });

    const onSubmit = async (data) => {
        setLoading(true);

        const userToAdd = {
            email: data.email,
            firstName: data.firstName,
            lastName: data.lastName,
            phoneNumber: data.phoneNumber,
            clientId: data.clientId,
            roles: ["Read-Write_User"]
        }

        try {
            const response = await addUser(userToAdd).unwrap()

            if (response.isSuccess) {
                dispatch(apiSlice.util.invalidateTags(["users"]))
                setLoading(false)
                toast({
                    title: "User Created Successfully!",
                    description: "You have successfully create a new User! ",
                })
            }
            else {
                toast({
                    title: "Creating the User was unsuccessfull!",
                    description: response.errorMessage,
                })
            }

            setOpen(false);

        } catch (err) {
            console.log(err);
        }
    };

    let content;
    if (isLoading) {
        content = <Loader />;
    } else if (isSuccess) {
        content = (
            <Drawer open={open} onOpenChange={setOpen}>
                <DrawerTrigger asChild>
                    <Button variant="outline" className="ml-auto mr- shadow">Create a new User</Button>
                </DrawerTrigger>
                <DrawerContent>
                    <div className="mx-auto w-full max-w-sm">
                        <DrawerHeader>
                            <DrawerTitle>Create User</DrawerTitle>
                        </DrawerHeader>
                        <Form {...form}>
                            <form onSubmit={form.handleSubmit(onSubmit)}>
                                <div className="space-y-2">
                                    <FormField
                                        control={form.control}
                                        name="email"
                                        render={({ field }) => (
                                            <FormItem className="text-left">
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
                                        name="firstName"
                                        render={({ field }) => (
                                            <FormItem className="text-left">
                                                <FormLabel>First Name</FormLabel>
                                                <FormControl>
                                                    <Input
                                                        {...field}
                                                        type="firstName"
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
                                            <FormItem className="text-left">
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
                                        name="phoneNumber"
                                        render={({ field }) => (
                                            <FormItem className="text-left">
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
                                        name="clientId"
                                        render={({ field }) => (
                                            <FormItem>
                                                <FormLabel>Client</FormLabel>
                                                <Select onValueChange={field.onChange} defaultValue={field.value}>
                                                    <FormControl>
                                                        <SelectTrigger>
                                                            <SelectValue placeholder="Select a Client for the User" />
                                                        </SelectTrigger>
                                                    </FormControl>
                                                    <SelectContent>
                                                        {clients.data.map((client, clientKey) =>
                                                            <SelectItem key={clientKey} value={client.id}>
                                                                {client.name}
                                                            </SelectItem>
                                                        )}
                                                    </SelectContent>
                                                </Select>
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
    } else if (isError) {
        content = <p>{JSON.stringify(error)}</p>;
    }

    return content
}

export default UserAddDrawer