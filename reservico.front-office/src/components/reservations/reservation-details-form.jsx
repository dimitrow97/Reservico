import { useGetReservationDetailsQuery } from "../../features/reservations/reservations-api-slice";
import { Input } from "../ui/input";
import { Label } from "../ui/label";
import Loader from "../common/loader";
import ReservationConfirmAlertDialog from "./reservation-confirm-alert-dialog"

const ReservationDetailsForm = (props) => {
    const {
        data,
        error,
        isError,
        isLoading,
        isSuccess } = useGetReservationDetailsQuery(props.id)

    let content;
    if (isLoading) {
        content = <Loader />;
    } else if (isSuccess) {    
        content = (
            <div>
                <div className="grid gap-4 py-4">
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
                            value={data.data.firstName + ' ' + data.data.lastName}
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
                        <Label htmlFor="phoneNumber" className="text-right col-span-2">
                            Phone Number
                        </Label>
                        <Input
                            id="phoneNumber"
                            value={data.data.phoneNumber}
                            className="col-span-10"
                            disabled
                        />
                    </div>
                    <div className="grid grid-cols-12 items-center gap-4">
                        <Label htmlFor="note" className="text-right col-span-2">
                            Note
                        </Label>
                        <Input
                            id="note"
                            value={data.data.note}
                            className="col-span-10"
                            disabled
                        />
                    </div>
                    <div className="grid grid-cols-12 items-center gap-4">
                        <Label htmlFor="guestsArrivingAt" className="text-right col-span-2">
                            Guests Arriving At
                        </Label>
                        <Input
                            id="guestsArrivingAt"
                            value={data.data.guestsArrivingAt}
                            className="col-span-10"
                            disabled
                        />
                    </div>
                    <div className="grid grid-cols-12 items-center gap-4">
                        <Label htmlFor="numberOfGuests" className="text-right col-span-2">
                            Number Of Guests
                        </Label>
                        <Input
                            id="numberOfGuests"
                            value={data.data.numberOfGuests}
                            className="col-span-10"
                            disabled
                        />
                    </div>
                    <div className="grid grid-cols-12 items-center gap-4">
                        <Label htmlFor="tableId" className="text-right col-span-2">
                            Table Id
                        </Label>
                        <Input
                            id="tableId"
                            value={data.data.tableId}
                            className="col-span-10"
                            disabled
                        />
                    </div>
                    <div className="grid grid-cols-12 items-center gap-4">
                        <Label htmlFor="tableName" className="text-right col-span-2">
                            Table Name
                        </Label>
                        <Input
                            id="tableName"
                            value={data.data.tableName}
                            className="col-span-10"
                            disabled
                        />
                    </div>
                    <div className="grid grid-cols-12 items-center gap-4">
                        <Label htmlFor="locationId" className="text-right col-span-2">
                            Location Id
                        </Label>
                        <Input
                            id="locationId"
                            value={data.data.locationId}
                            className="col-span-10"
                            disabled
                        />
                    </div>
                    <div className="grid grid-cols-12 items-center gap-4">
                        <Label htmlFor="locationName" className="text-right col-span-2">
                            Location Name
                        </Label>
                        <Input
                            id="locationName"
                            value={data.data.locationName}
                            className="col-span-10"
                            disabled
                        />
                    </div>
                </div>
                <div className="flex flex-row justify-center gap-4">
                    <ReservationConfirmAlertDialog props={data.data} />
                </div>                
            </div>
        )
    } else if (isError) {
        content = <p>{JSON.stringify(error)}</p>;
    }

    return content
}

export default ReservationDetailsForm