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
import { useDeleteLocationImageMutation } from "../../../features/locations/images/images-api-slice"
import { useToast } from "@/components/ui/use-toast"
import { useState } from 'react'
import { apiSlice } from "../../../app/api/api-slice"
import { useDispatch } from "react-redux"

export function LocationImageDeleteAlertDialog({ locationImageId }) {
    const [deleteLocationImage] = useDeleteLocationImageMutation()
    const [open, setOpen] = useState(false)
    const { toast } = useToast()   
    const dispatch = useDispatch()
   
    const handleOnClick = async (e) => {
        e.preventDefault()

        try {
            const response = await deleteLocationImage({ locationImageId: locationImageId }).unwrap()

            if (response.isSuccess) {
                dispatch(apiSlice.util.invalidateTags(["images"]))                
            }
            else {
                toast({
                    title: "Delete was unsuccessfull!",
                    description: response.errorMessage,
                })
            }
        } catch (err) {
        }

        setOpen(false)
    };

    return (
        <AlertDialog open={open} onOpenChange={setOpen}>
            <AlertDialogTrigger asChild>
                <Button variant="outline" className="w-1/8 p-2">
                    <Trash2 className="m-auto h-4 w-4" />
                </Button>
            </AlertDialogTrigger>
            <AlertDialogContent>
                <AlertDialogHeader>
                    <AlertDialogTitle>Are you absolutely sure?</AlertDialogTitle>
                    <AlertDialogDescription>
                        This action can be undone. This will soft-delete
                        the location image and keep the data on our servers.
                    </AlertDialogDescription>
                </AlertDialogHeader>
                <AlertDialogFooter>
                    <AlertDialogCancel>Cancel</AlertDialogCancel>
                    <AlertDialogAction onClick={handleOnClick} className="bg-red-500 hover:bg-red-700">
                        <Trash2 className="mr-2 h-4 w-4" />Continue
                    </AlertDialogAction>
                </AlertDialogFooter>
            </AlertDialogContent>
        </AlertDialog>
    )
}

export default LocationImageDeleteAlertDialog