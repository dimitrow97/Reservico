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
import { useAddUserClientMutation } from "../../features/users/users-api-slice"
import { useGetClientsQuery } from "../../features/clients/clients-api-slice"
import { UserAddClientSchema } from "../../schema";
import { apiSlice } from "../../app/api/api-slice"
import { useDispatch } from "react-redux"
import Loader from "../common/loader"

export function UserAddClientDrawer({ userId }) {
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
    const [addClient] = useAddUserClientMutation()

    const form = useForm({
        resolver: zodResolver(UserAddClientSchema),
        defaultValues: {
            clientId: "",
        },
    });

    const onSubmit = async (data) => {
        setLoading(true);

        const clientToAdd = {
            userId: userId,
            clientId: data.clientId
        }

        try {
            const response = await addClient(clientToAdd).unwrap()

            if (response.isSuccess) {
                dispatch(apiSlice.util.invalidateTags(["client-users"]))
                setLoading(false)
                toast({
                    title: "Client was Added Successfully!",
                    description: "You have successfully added a Client for the User! ",
                })
            }
            else {
                toast({
                    title: "Adding the Client was unsuccessfull!",
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
                    <Button variant="outline" className="ml-2 mr-shadow">Add Client</Button>
                </DrawerTrigger>
                <DrawerContent>
                    <div className="mx-auto w-full max-w-sm">
                        <DrawerHeader>
                            <DrawerTitle>Add Client for User</DrawerTitle>
                        </DrawerHeader>
                        <Form {...form}>
                            <form onSubmit={form.handleSubmit(onSubmit)}>
                                <div className="space-y-2">                                    
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

export default UserAddClientDrawer