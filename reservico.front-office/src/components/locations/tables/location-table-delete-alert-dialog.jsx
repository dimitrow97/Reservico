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
import { useDeleteTableMutation } from "../../../features/locations/tables/tables-api-slice"
import { useToast } from "@/components/ui/use-toast"
import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { apiSlice } from "../../../app/api/api-slice"
import { useDispatch } from "react-redux"

export function LocationTableDeleteAlertDialog({ props }) {
    const [deleteLocationTable] = useDeleteTableMutation()
    const { toast } = useToast()    
    const navigate = useNavigate()
    const dispatch = useDispatch()

    const [id] = useState(props.id)
   
    const handleOnClick = async (e) => {
        e.preventDefault()

        try {
            const response = await deleteLocationTable({ tableId: id }).unwrap()

            if (response.isSuccess) {
                dispatch(apiSlice.util.invalidateTags(["tables"]))
                navigate("/locations")
            }
            else {
                toast({
                    title: "Delete was unsuccessfull!",
                    description: response.errorMessage,
                })
            }
        } catch (err) {
            console.log(err);
        }
    };

    return (
        <AlertDialog>
            <AlertDialogTrigger asChild>
                <Button variant="outline" className="w-1/4 p-2">Delete</Button>
            </AlertDialogTrigger>
            <AlertDialogContent>
                <AlertDialogHeader>
                    <AlertDialogTitle>Are you absolutely sure?</AlertDialogTitle>
                    <AlertDialogDescription>
                        This action can be undone. This will soft-delete
                        the location table and keep the data on our servers.
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

export default LocationTableDeleteAlertDialog