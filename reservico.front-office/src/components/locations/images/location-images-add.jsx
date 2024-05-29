import { Button } from '@/components/ui/button'
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { useUploadImageMutation } from "../../../features/locations/images/images-api-slice"
import { useToast } from "@/components/ui/use-toast"
import { useState } from 'react'
import { apiSlice } from "../../../app/api/api-slice"
import { useDispatch, useSelector } from "react-redux"

const LocationImagesAdd = ({ locationId }) => {
    const [upload] = useUploadImageMutation()
    const [loading, setLoading] = useState(false)
    const [file, setFile] = useState()
    const [inputKey, setInputKey] = useState(Date.now())
    const { toast } = useToast()
    const dispatch = useDispatch()

    const saveFile = (e) => {
        setFile(e.target.files[0])
    }

    const uploadImage = async () => {
        setLoading(true);

        var bodyFormData = new FormData()
        bodyFormData.append("locationId", locationId)
        bodyFormData.append('file', file)

        try {
            const response = await upload(bodyFormData).unwrap()

            if (response.isSuccess) {
                dispatch(apiSlice.util.invalidateTags(["images"]))
                setLoading(false)
                toast({
                    title: "Image was Uploaded Successfully!",
                    description: "You have successfully uploaded a new Image! ",
                })
            }
            else {
                toast({
                    title: "Uploading the Image was unsuccessfull!",
                    description: response.errorMessage,
                })
            }

            setFile(null)
        } catch (err) {
            toast({
                title: "Uploading the Image was unsuccessfull!",
                description: err.data.errorMessage,
            })
        }

        setInputKey(Date.now())
    }

    return (
        <div className="flex flex-row pb-4">
            <div className="grid max-w-sm items-center gap-1.5">
                <Label htmlFor="image">Image</Label>
                <Input id="image" type="file" key={inputKey} onChange={saveFile} />
            </div>
            <Button className="mt-auto ml-2 bg-green-500 hover:bg-green-600" type="button" onClick={uploadImage}>
                <span>{loading ? "Loading..." : "Upload"}</span>
            </Button>
        </div>
    )
}

export default LocationImagesAdd