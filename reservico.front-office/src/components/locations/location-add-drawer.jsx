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
import CategoriesMultiSelect from "../categories/categories-multi-select"
import { useToast } from "@/components/ui/use-toast"
import { useState } from 'react'
import { useAddLocationMutation } from "../../features/locations/locations-api-slice"
import { LocationAddSchema } from "../../schema";
import { apiSlice } from "../../app/api/api-slice"
import { useDispatch, useSelector } from "react-redux"
import { selectCurrentClient } from "../../features/auth/auth-slice"
import { useGetCategoriesQuery } from "../../features/categories/categories-api-slice"
import Loader from "../common/loader"

export function LocationAddDrawer() {
    const [loading, setLoading] = useState(false)
    const [open, setOpen] = useState(false)    
    const [selected, setSelected] = React.useState([])
    const { toast } = useToast()
    const dispatch = useDispatch()
    const [addLocation] = useAddLocationMutation()
    const currentClient = useSelector(selectCurrentClient)

    const {
        data: categories,
        isLoading,
        isSuccess,
        isError,
        error
    } = useGetCategoriesQuery()   

    const form = useForm({
        resolver: zodResolver(LocationAddSchema),
        defaultValues: {
            name: "",
            address: "",
            city: "",
            email: "",
            postcode: "",
            country: ""
        },
    });

    const onSubmit = async (data) => {
        setLoading(true);

        let selectedCategories = selected.map(a => a.categoryId);

        const locationToAdd = {
            name: data.name,
            address: data.address,
            city: data.city,
            email: data.email,
            postcode: data.postcode,
            country: data.country,
            clientId: currentClient,
            categories: selectedCategories
        }

        try {
            const response = await addLocation(locationToAdd).unwrap()

            if (response.isSuccess) {
                dispatch(apiSlice.util.invalidateTags(["locations"]))
                setLoading(false)
                toast({
                    title: "Location Created Successfully!",
                    description: "You have successfully create a new Location! ",
                })
            }
            else {
                toast({
                    title: "Creating the Location was unsuccessfull!",
                    description: response.errorMessage,
                })
            }

            setOpen(false);

            form.reset({
                name: "",
                address: "",
                city: "",
                email: "",
                postcode: "",
                country: ""
            })
            setSelected([])

        } catch (err) {
            toast({
                title: "Creating the Location was unsuccessfull!",
                description: err.data.errorMessage,
            })
        }
    };

    let content;
    if (isLoading) {
        content = <Loader />;
    } else if (isSuccess) {
        content = (
            <Drawer open={open} onOpenChange={setOpen}>
                <DrawerTrigger asChild>
                    <Button variant="outline" className="ml-auto mr- shadow">Create a new Location</Button>
                </DrawerTrigger>
                <DrawerContent>
                    <div className="mx-auto w-full max-w-4xl">
                        <DrawerHeader>
                            <DrawerTitle>Create Location</DrawerTitle>
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
                                    </div>
                                    <div className="px-8">
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
                                            name="categories"
                                            render={({ field }) => (
                                                <FormItem>
                                                    <FormLabel>Categories</FormLabel>
                                                    <FormControl>
                                                        <CategoriesMultiSelect
                                                            categories={categories.data}
                                                            selected={selected}
                                                            setSelected={setSelected}
                                                        />
                                                    </FormControl>
                                                    <FormMessage />
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
    } else if (isError) {
        content = <p>{JSON.stringify(error)}</p>;
    }

    return content
}

export default LocationAddDrawer