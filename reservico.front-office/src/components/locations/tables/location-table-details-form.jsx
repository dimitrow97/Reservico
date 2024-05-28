import { useGetTableQuery } from "../../../features/locations/tables/tables-api-slice"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Checkbox } from "@/components/ui/checkbox"
import Loader from "../../common/loader"
import LocationTableDeleteAlertDialog from "./location-table-delete-alert-dialog"

const LocationTableDetailsForm = (props) => {
    const {
        data,
        error,
        isError,
        isLoading,
        isSuccess } = useGetTableQuery(props.id)

    let content;
    if (isLoading) {
        content = <Loader />;
    } else if (isSuccess) {
        content = (
            <div>
                <div className="grid grid-cols-2">
                    <div className="grid grid-cols-1 gap-x-8 gap-y-4 py-4">
                        <div className="grid grid-cols-12 items-center gap-4">
                            <Label htmlFor="id" className="text-right col-span-2">
                                Id
                            </Label>
                            <Input
                                id="id"
                                value={data.data.id}
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
                            <Label htmlFor="description" className="text-right col-span-2">
                                Description
                            </Label>
                            <Input
                                id="description"
                                value={data.data.description}
                                className="col-span-10"
                                disabled
                            />
                        </div>                        
                        <div className="grid grid-cols-12 items-center gap-4">
                            <Label htmlFor="locationName" className="text-right col-span-2">
                                Location
                            </Label>
                            <Input
                                id="locationName"
                                value={data.data.locationName}
                                className="col-span-10"
                                disabled
                            />
                        </div>
                        <div className="grid grid-cols-12 items-center gap-4">
                            <Label htmlFor="capacity" className="text-right col-span-2">
                                Capacity
                            </Label>
                            <Input
                                id="capacity"
                                value={data.data.capacity}
                                className="col-span-10"
                                disabled
                            />
                        </div>
                    </div>
                    <div className="grid gap-x-8 gap-y-4 py-4 pl-8">
                        <div className="grid grid-cols-12 items-center gap-4">
                            <Label htmlFor="workingHoursFrom" className="text-right col-span-2">
                                Working Hours From
                            </Label>
                            <Input
                                id="workingHoursFrom"
                                value={data.data.workingHoursFrom}
                                className="col-span-10"
                                disabled
                            />
                        </div>
                        <div className="grid grid-cols-12 items-center gap-4">
                            <Label htmlFor="workingHoursTo" className="text-right col-span-2">
                                Working Hours To
                            </Label>
                            <Input
                                id="workingHoursTo"
                                value={data.data.workingHoursTo}
                                className="col-span-10"
                                disabled
                            />
                        </div>                        
                        <div className="grid grid-cols-12 items-center gap-4">
                            <Label htmlFor="tableTurnOffset" className="text-right col-span-2">
                                Table Turn Offset
                            </Label>
                            <Input
                                id="tableTurnOffset"
                                value={data.data.tableTurnOffset}
                                className="col-span-10"
                                disabled
                            />
                        </div>
                        <div className="grid items-center gap-4">
                            <div className="flex items-center pl-8 space-x-2">
                                <Checkbox id="terms2" value={data.data.canTableTurn} disabled />
                                <label
                                    htmlFor="terms2"
                                    className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70"
                                >
                                    Can the Table be turned?
                                </label>
                            </div>
                        </div>                        
                    </div>
                </div>
                <div className="flex flex-row justify-center gap-4">
                    <LocationTableDeleteAlertDialog props={data.data} />
                </div>
            </div>
        )
    } else if (isError) {
        content = <p>{JSON.stringify(error)}</p>;
    }

    return content
}

export default LocationTableDetailsForm