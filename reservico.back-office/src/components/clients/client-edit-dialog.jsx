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
    SquarePen
} from "lucide-react"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { useUpdateClientMutation } from "../../features/clients/clients-api-slice"
import { useToast } from "@/components/ui/use-toast"
import { useState } from 'react'

export function ClientEditDialog(props) {
    const [updateClient] = useUpdateClientMutation()
    const { toast } = useToast()
    const [open, setOpen] = useState(false);

    const [id] = useState(props.props.id)
    const [name, setName] = useState(props.props.name)
    const [address, setAddress] = useState(props.props.address)
    const [city, setCity] = useState(props.props.city)
    const [postcode, setPostcode] = useState(props.props.postcode)
    const [country, setCountry] = useState(props.props.country)

    const handleNameInput = (e) => setName(e.target.value)
    const handleAddressInput = (e) => setAddress(e.target.value)
    const handleCityInput = (e) => setCity(e.target.value)
    const handlePostcodeInput = (e) => setPostcode(e.target.value)
    const handleCountryInput = (e) => setCountry(e.target.value)

    const onSubmit = async (e) => {
        e.preventDefault()

        const clientToUpdate = {
            id: id,
            name: name,
            address: address,
            city: city,
            postCode: postcode,
            country: country,
        }

        try {
            const response = await updateClient(clientToUpdate).unwrap()

            console.log(response)

            if (response.isSuccess) {
                toast({
                    title: "Update Complete!",
                    description: "You have successfully updated Client with Id: " + id,
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

    return (
        <Dialog open={open} onOpenChange={setOpen}>
            <DialogTrigger asChild>
                <Button variant="outline" className="w-1/4 p-2">Edit Client</Button>
            </DialogTrigger>
            <DialogContent className="sm:max-w-[650px]">
                <DialogHeader>
                    <DialogTitle className="drop-shadow-lg">
                        Edit Client
                    </DialogTitle>
                    <DialogDescription>
                        Make changes to the client here. Click save when you're done.
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
}

export default ClientEditDialog