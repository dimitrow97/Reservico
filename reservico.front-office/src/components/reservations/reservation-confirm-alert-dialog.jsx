import {
    AlertDialog,
    AlertDialogAction,
    AlertDialogCancel,
    AlertDialogContent,
    AlertDialogDescription,
    AlertDialogFooter,
    AlertDialogHeader,
    AlertDialogTitle,
    AlertDialogTrigger,
} from "@/components/ui/alert-dialog"
import { Button } from "@/components/ui/button"
import {
    Trash2
} from "lucide-react"
import { useConfirmReservationMutation } from "../../features/reservations/reservations-api-slice"
import { useToast } from "@/components/ui/use-toast"
import { useState } from 'react'
import { apiSlice } from "../../app/api/api-slice"
import { useDispatch } from "react-redux"

export function ReservationConfirmAlertDialog(props) {
    const [confirm] = useConfirmReservationMutation()
    const [open, setOpen] = useState(false) 
    const { toast } = useToast()    
    const dispatch = useDispatch()

    const [id] = useState(props.props.id)
   
    const handleOnClick = async (e) => {
        e.preventDefault()

        try {
            const response = await confirm({ reservationId: id }).unwrap()

            if (response.isSuccess) {
                dispatch(apiSlice.util.invalidateTags(["reservations"]))
                toast({
                    title: "Confirmation was successfull!",
                    description: "You have successfully confirmed the reservation!",
                })
            }
            else {
                toast({
                    title: "Confirmation was unsuccessfull!",
                    description: response.errorMessage,
                })
            }

            setOpen(false)
        } catch (err) {
            toast({
                title: "Confirmation was unsuccessfull!",
                description: err.data.errorMessage,
            })
        }
    };

    return (
        <AlertDialog open={open} onOpenChange={setOpen}>
            <AlertDialogTrigger asChild>
                <Button variant="outline" className="w-1/4 p-2">Confirm</Button>
            </AlertDialogTrigger>
            <AlertDialogContent>
                <AlertDialogHeader>
                    <AlertDialogTitle>Are you sure?</AlertDialogTitle>
                    <AlertDialogDescription>
                        This action cannot be undone. This will confirm
                        the reservation and send a confirmation email to the client, who requested it.
                    </AlertDialogDescription>
                </AlertDialogHeader>
                <AlertDialogFooter>
                    <AlertDialogCancel>Cancel</AlertDialogCancel>
                    <AlertDialogAction onClick={handleOnClick} className="bg-green-500 hover:bg-green-700">
                        <Trash2 className="mr-2 h-4 w-4" />Continue
                    </AlertDialogAction>
                </AlertDialogFooter>
            </AlertDialogContent>
        </AlertDialog>
    )
}

export default ReservationConfirmAlertDialog