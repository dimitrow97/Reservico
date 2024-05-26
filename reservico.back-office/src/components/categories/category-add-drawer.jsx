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
import { useAddCategoryMutation } from "../../features/categories/categories-api-slice"
import { CategoryAddSchema } from "../../schema";
import { apiSlice } from "../../app/api/api-slice"
import { useDispatch } from "react-redux"

export function CategoryAddDrawer() {
    const [loading, setLoading] = useState(false);
    const [open, setOpen] = useState(false);
    const { toast } = useToast()
    const dispatch = useDispatch()
    const [addCategory] = useAddCategoryMutation()

    const form = useForm({
        resolver: zodResolver(CategoryAddSchema),
        defaultValues: {
            name: ""
        },
    });

    const onSubmit = async (data) => {
        setLoading(true);

        const categoryToAdd = {
            name: data.name
        }

        try {
            const response = await addCategory(categoryToAdd).unwrap()

            if (response.isSuccess) {
                dispatch(apiSlice.util.invalidateTags(["category"]))
                setLoading(false)
                toast({
                    title: "Category Created Successfully!",
                    description: "You have successfully create a new Category! ",
                })
            }
            else {
                toast({
                    title: "Creating the Category was unsuccessfull!",
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
                <Button variant="outline" className="ml-auto mr- shadow">Create a new Category</Button>
            </DrawerTrigger>
            <DrawerContent>
                <div className="mx-auto w-full max-w-sm">
                    <DrawerHeader>
                        <DrawerTitle>Create Category</DrawerTitle>
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

export default CategoryAddDrawer