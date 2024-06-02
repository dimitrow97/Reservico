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
import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
    CardFooter
} from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import {
    Trash2
} from "lucide-react"
import { useCancelReservationMutation } from "../../features/reservations/reservations-api-slice"
import { useToast } from "@/components/ui/use-toast"
import { useState } from 'react'
import { apiSlice } from "../../app/api/api-slice"
import { useDispatch } from "react-redux"
import Error from "../common/error"

export function ReservationCancelAlertDialog({ id }) {
    const [cancel] = useCancelReservationMutation()
    const [open, setOpen] = useState(true)
    const { toast } = useToast()
    const dispatch = useDispatch()
    const [error, setError] = useState(false)
    const [showSuccess, setShowSuccess] = useState(false)

    const handleOnClick = async (e) => {
        e.preventDefault()

        try {
            const response = await cancel({ reservationId: id }).unwrap()

            if (response.isSuccess) {
                setShowSuccess(true)
                dispatch(apiSlice.util.invalidateTags(["reservations"]))
                toast({
                    title: "Cancelling the reservation was successfull!",
                    description: "You have successfully cancelled the reservation!",
                })
            }
            else {
                setError(true)
                toast({
                    title: "Cancelling was unsuccessfull!",
                    description: response?.errorMessage,
                })
            }
        } catch (err) {
            setError(true)
            toast({
                title: "Cancelling was unsuccessfull!",
                description: err?.data?.errorMessage,
            })
        }

        setOpen(false)
    };

    return (
        <div>
            {error ?
                <Error /> : ""                
            }

            {showSuccess ?
                (<div className="rounded-lg p-8 text-white flex flex-col items-center justify-center">
                    <Card x-chunk="dashboard-01-chunk-0" className="border-0 shadow-none">
                        <CardContent className="flex flex-col items-center justify-center text-center">
                            <div className="text-3xl p-4 text-slate-600 font-bold border-4 border-slate-600 border-spacing-4">Reservation was cancelled!</div>
                            <p className="text-lg text-slate-500 pt-4 w-2/3">
                                Sad to see you cancelling, but we hope to see you again!
                            </p>
                        </CardContent>
                    </Card>
                </div>)
                : ""
            }

            <AlertDialog open={open} onOpenChange={setOpen}>
                <AlertDialogContent>
                    <AlertDialogHeader>
                        <AlertDialogTitle>Are you sure you want to Cancel?</AlertDialogTitle>
                        <AlertDialogDescription>
                            This will permanently cancel the reservation.
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
        </div>
    )
}

export default ReservationCancelAlertDialog