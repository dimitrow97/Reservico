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
import { useUpdateUserMutation } from "../../features/users/users-api-slice"
import { useToast } from "@/components/ui/use-toast"
import { useState } from 'react'
import { apiSlice } from "../../app/api/api-slice"
import { useDispatch } from "react-redux"

export function UserEditDialog(props) {
    const [updateUser] = useUpdateUserMutation()
    const { toast } = useToast()
    const [open, setOpen] = useState(false);    
    const dispatch = useDispatch()

    const [id] = useState(props.props.userId)
    const [firstName, setFirstName] = useState(props.props.firstName)
    const [lastName, setLastName] = useState(props.props.lastName)
    const [email, setEmail] = useState(props.props.email)
    const [phoneNumber, setPhoneNumber] = useState(props.props.phoneNumber)

    const handleFirstNameInput = (e) => setFirstName(e.target.value)
    const handleLastNameInput = (e) => setLastName(e.target.value)
    const handleEmailInput = (e) => setEmail(e.target.value)
    const handlePhoneNumberInput = (e) => setPhoneNumber(e.target.value)

    const onSubmit = async (e) => {
        e.preventDefault()

        const userToUpdate = {
            userId: id,
            email: email,
            firstName: firstName,
            lastName: lastName,
            salutation: props.props.salutation,
            phoneNumber: phoneNumber,
            isActive: props.props.isActive,
            isUsingDefaultPassword: props.props.isUsingDefaultPassword,
            roles: props.props.roles,
            clients: props.props.clients
        }

        try {
            const response = await updateUser(userToUpdate).unwrap()

            if (response.isSuccess) {
                dispatch(apiSlice.util.invalidateTags(["users"]))
                toast({
                    title: "Update Complete!",
                    description: "You have successfully updated User with Id: " + id,
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
                <Button variant="outline" className="w-1/4 p-2">Edit User</Button>
            </DialogTrigger>
            <DialogContent className="sm:max-w-[650px]">
                <DialogHeader>
                    <DialogTitle className="drop-shadow-lg">
                        Edit User
                    </DialogTitle>
                    <DialogDescription>
                        Make changes to the user here. Click save when you're done.
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
                            <Label htmlFor="firstName" className="text-right col-span-2">
                                First Name
                            </Label>
                            <Input
                                id="firstName"
                                className="col-span-10"
                                onChange={handleFirstNameInput}
                                value={firstName}
                                required
                            />
                        </div>
                        <div className="grid grid-cols-12 items-center gap-4">
                            <Label htmlFor="lastName" className="text-right col-span-2">
                                Last Name
                            </Label>
                            <Input
                                id="lastName"
                                className="col-span-10"
                                onChange={handleLastNameInput}
                                value={lastName}
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
                            />
                        </div>
                        <div className="grid grid-cols-12 items-center gap-4">
                            <Label htmlFor="phoneNumber" className="text-right col-span-2">
                                Phone Number
                            </Label>
                            <Input
                                id="phoneNumber"
                                className="col-span-10"
                                onChange={handlePhoneNumberInput}
                                value={phoneNumber}
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

export default UserEditDialog