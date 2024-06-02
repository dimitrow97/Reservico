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
import { useCancellationReservationMutation } from "../../features/reservations/reservations-api-slice"
import { useToast } from "@/components/ui/use-toast"
import { useState } from 'react'
import { apiSlice } from "../../app/api/api-slice"
import { useDispatch } from "react-redux"

export function ReservationCancellationAlertDialog(props) {
    const [cancellation] = useCancellationReservationMutation()
    const [open, setOpen] = useState(false)
    const { toast } = useToast()
    const dispatch = useDispatch()

    const [id] = useState(props.props.id)

    const handleOnClick = async (e) => {
        e.preventDefault()

        try {
            const response = await cancellation({ reservationId: id }).unwrap()

            if (response.isSuccess) {
                dispatch(apiSlice.util.invalidateTags(["reservations"]))
                toast({
                    title: "An Email for Cancelling the reservation was sent!",
                    description: "Visit your Inbox and use the provided link to cancel the reservation!",
                })
            }
            else {
                toast({
                    title: "Cancelling was unsuccessfull!",
                    description: response?.errorMessage,
                })
            }
        } catch (err) {
            toast({
                title: "Cancelling was unsuccessfull!",
                description: err?.data?.errorMessage,
            })
        } 

        setOpen(false)
    };

    return (
        <AlertDialog open={open} onOpenChange={setOpen}>
            <AlertDialogTrigger asChild>
                <Button variant="outline" className="w-full p-2 bg-[#28282a] hover:border-2 text-white hover:bg-white hover:text-black">
                    Cancel Reservation
                </Button>
            </AlertDialogTrigger>
            <AlertDialogContent>
                <AlertDialogHeader>
                    <AlertDialogTitle>Are you sure you want to Cancel?</AlertDialogTitle>
                    <AlertDialogDescription>
                        This will send you an email
                        containing a link to cancel the reservation.                        
                    </AlertDialogDescription>
                </AlertDialogHeader>
                <AlertDialogFooter>
                    <AlertDialogCancel>Close</AlertDialogCancel>
                    <AlertDialogAction onClick={handleOnClick} className="bg-[#28282a] border-2 hover:border-2 hover:bg-white hover:text-black">
                        <Trash2 className="mr-2 h-4 w-4" />Confirm
                    </AlertDialogAction>
                </AlertDialogFooter>
            </AlertDialogContent>
        </AlertDialog>
    )
}

export default ReservationCancellationAlertDialog