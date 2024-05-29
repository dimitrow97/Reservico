import { Button } from "@/components/ui/button"
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
    DialogTrigger,
} from "@/components/ui/dialog"
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from "@/components/ui/form"
import {
    SquarePen
} from "lucide-react"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { useUpdateLocationMutation } from "../../features/locations/locations-api-slice"
import { useGetCategoriesQuery } from "../../features/categories/categories-api-slice"
import { useToast } from "@/components/ui/use-toast"
import { useState } from 'react'
import { apiSlice } from "../../app/api/api-slice"
import { useDispatch } from "react-redux"
import CategoriesMultiSelect from "../categories/categories-multi-select"
import Loader from "../common/loader"

export function LocationEditDialog({ props }) {
    const [updateLocation] = useUpdateLocationMutation()
    const { toast } = useToast()
    const [open, setOpen] = useState(false);
    const dispatch = useDispatch()

    const {
        data: categories,
        isLoading,
        isSuccess,
        isError,
        error
    } = useGetCategoriesQuery()

    const [id] = useState(props.locationId)
    const [name, setName] = useState(props.name)
    const [email, setEmail] = useState(props.email)
    const [address, setAddress] = useState(props.address)
    const [city, setCity] = useState(props.city)
    const [postcode, setPostcode] = useState(props.postcode)
    const [country, setCountry] = useState(props.country)
    const [selected, setSelected] = useState(props.categories)

    const handleNameInput = (e) => setName(e.target.value)
    const handleEmailInput = (e) => setEmail(e.target.value)
    const handleAddressInput = (e) => setAddress(e.target.value)
    const handleCityInput = (e) => setCity(e.target.value)
    const handlePostcodeInput = (e) => setPostcode(e.target.value)
    const handleCountryInput = (e) => setCountry(e.target.value)

    const onSubmit = async (e) => {
        e.preventDefault()

        let selectedCategories = selected?.map(a => a.categoryId) ?? [];

        const locationToUpdate = {
            locationId: id,
            name: name,
            email: email,
            address: address,
            city: city,
            postCode: postcode,
            country: country,
            categories: selectedCategories
        }

        try {
            const response = await updateLocation(locationToUpdate).unwrap()

            if (response.isSuccess) {
                dispatch(apiSlice.util.invalidateTags(["locations"]))
                toast({
                    title: "Update Complete!",
                    description: "You have successfully updated Location with Id: " + id,
                })
            }
            else {
                toast({
                    title: "Update was unsuccessfull!",
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
            <Dialog open={open} onOpenChange={setOpen}>
                <DialogTrigger asChild>
                    <Button variant="outline" className="w-1/4 p-2">Edit Location</Button>
                </DialogTrigger>
                <DialogContent className="sm:max-w-[650px]">
                    <DialogHeader>
                        <DialogTitle className="drop-shadow-lg">
                            Edit Location
                        </DialogTitle>
                        <DialogDescription>
                            Make changes to the location here. Click save when you're done.
                        </DialogDescription>
                    </DialogHeader>
                    <form onSubmit={onSubmit}>
                        <div className="grid gap-4 py-4">
                            <div className="grid grid-cols-12 items-center gap-4">
                                <Label htmlFor="id" className="text-right col-span-2">
                                    Id
                                </Label>
                                <Input
                                    id="id"
                                    className="col-span-10"
                                    disabled
                                    value={id}
                                />
                            </div>
                            <div className="grid grid-cols-12 items-center gap-4">
                                <Label htmlFor="name" className="text-right col-span-2">
                                    Name
                                </Label>
                                <Input
                                    id="name"
                                    className="col-span-10"
                                    onChange={handleNameInput}
                                    value={name}
                                    required
                                />
                            </div>
                            <div className="grid grid-cols-12 items-center gap-4">
                                <Label htmlFor="email" className="text-right col-span-2">
                                    Email
                                </Label>
                                <Input
                                    id="email"
                                    className="col-span-10"
                                    onChange={handleEmailInput}
                                    value={email}
                                    required
                                />
                            </div>
                            <div className="grid grid-cols-12 items-center gap-4">
                                <Label htmlFor="address" className="text-right col-span-2">
                                    Address
                                </Label>
                                <Input
                                    id="address"
                                    className="col-span-10"
                                    onChange={handleAddressInput}
                                    value={address}
                                />
                            </div>
                            <div className="grid grid-cols-12 items-center gap-4">
                                <Label htmlFor="city" className="text-right col-span-2">
                                    City
                                </Label>
                                <Input
                                    id="city"
                                    className="col-span-10"
                                    onChange={handleCityInput}
                                    value={city}
                                />
                            </div>
                            <div className="grid grid-cols-12 items-center gap-4">
                                <Label htmlFor="postcode" className="text-right col-span-2">
                                    Post Code
                                </Label>
                                <Input
                                    id="postcode"
                                    className="col-span-10"
                                    onChange={handlePostcodeInput}
                                    value={postcode}
                                />
                            </div>
                            <div className="grid grid-cols-12 items-center gap-4">
                                <Label htmlFor="country" className="text-right col-span-2">
                                    Country
                                </Label>
                                <Input
                                    id="country"
                                    className="col-span-10"
                                    onChange={handleCountryInput}
                                    value={country}
                                />
                            </div>
                            <div className="items-center gap-4">
                                <Label htmlFor="categories" className="text-right col-span-2">
                                    Categories
                                </Label>
                                <CategoriesMultiSelect
                                    categories={categories.data}
                                    selected={selected}
                                    setSelected={setSelected}
                                />
                            </div>
                        </div>
                        <DialogFooter>
                            <Button type="submit" className="bg-green-500 hover:bg-green-600">
                                <SquarePen className="mr-2 h-4 w-4" />Save changes
                            </Button>
                        </DialogFooter>
                    </form>
                </DialogContent>
            </Dialog>
        )
    } else if (isError) {
        content = <p>{JSON.stringify(error)}</p>;
    }

    return content
}

export default LocationEditDialog