import { useGetLocationDetailsQuery } from "../../features/locations/locations-api-slice"
import { Input } from "../ui/input";
import { Label } from "../ui/label";
import Loader from "../common/loader";
import { Badge } from "@/components/ui/badge"
import LocationDeleteAlertDialog from './location-delete-alert-dialog'
import LocationEditDialog from "./location-edit-dialog"

const LocationDetailsForm = (props) => {
    const {
        data,
        error,
        isError,
        isLoading,
        isSuccess } = useGetLocationDetailsQuery(props.id)

    let content;
    if (isLoading) {
        content = <Loader />;
    } else if (isSuccess) {
        content = (
            <div>
                <div className="grid grid-cols-2">
                    <div className="grid grid-cols-1 gap-4 py-4">
                        <div className="grid grid-cols-12 items-center gap-4">
                            <Label htmlFor="id" className="text-right col-span-2">
                                Id
                            </Label>
                            <Input
                                id="id"
                                value={data.data.locationId}
                                className="col-span-10"
                                disabled
                            />
                        </div>
                        <div className="grid grid-cols-12 items-center gap-4">
                            <Label htmlFor="name" className="text-right col-span-2">
                                Name
                            </Label>
                            <Input
                                id="name"
                                value={data.data.name}
                                className="col-span-10"
                                disabled
                            />
                        </div>
                        <div className="grid grid-cols-12 items-center gap-4">
                            <Label htmlFor="email" className="text-right col-span-2">
                                Email
                            </Label>
                            <Input
                                id="email"
                                value={data.data.email}
                                className="col-span-10"
                                disabled
                            />
                        </div>
                        <div className="grid grid-cols-12 items-center gap-4">
                            <Label htmlFor="address" className="text-right col-span-2">
                                Address
                            </Label>
                            <Input
                                id="address"
                                value={data.data.address}
                                className="col-span-10"
                                disabled
                            />
                        </div>
                        <div className="grid grid-cols-12 items-center gap-4">
                            <Label htmlFor="city" className="text-right col-span-2">
                                City
                            </Label>
                            <Input
                                id="city"
                                value={data.data.city}
                                className="col-span-10"
                                disabled
                            />
                        </div>
                    </div>
                    <div className="grid gap-4 py-4">
                        <div className="grid grid-cols-12 items-center gap-4">
                            <Label htmlFor="postcode" className="text-right col-span-2">
                                Post Code
                            </Label>
                            <Input
                                id="postcode"
                                value={data.data.postcode}
                                className="col-span-10"
                                disabled
                            />
                        </div>
                        <div className="grid grid-cols-12 items-center gap-4">
                            <Label htmlFor="country" className="text-right col-span-2">
                                Country
                            </Label>
                            <Input
                                id="country"
                                value={data.data.country}
                                className="col-span-10"
                                disabled
                            />
                        </div>
                        <div className="grid grid-cols-12 items-center gap-4">
                            <Label htmlFor="client" className="text-right col-span-2">
                                Client
                            </Label>
                            <Input
                                id="client"
                                value={data.data.clientName}
                                className="col-span-10"
                                disabled
                            />
                        </div>
                        <div className="grid grid-cols-12 items-center gap-4">
                            <Label htmlFor="categories" className="text-right col-span-2">
                                Categories
                            </Label>
                            <div>
                                {data.data.categories.map((cat, key) => (
                                    <Badge key={key} variant="secondary">{cat.name}</Badge>
                                ))}
                            </div>
                        </div>
                    </div>
                </div>
                <div className="flex flex-row justify-center gap-4">
                    <LocationEditDialog props={data.data} />
                    <LocationDeleteAlertDialog props={data.data} />
                </div>
            </div>
        )
    } else if (isError) {
        content = <p>{JSON.stringify(error)}</p>;
    }

    return content
}

export default LocationDetailsForm