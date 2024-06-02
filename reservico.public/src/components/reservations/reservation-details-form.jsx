import { useGetReservationDetailsQuery } from "../../features/reservations/reservations-api-slice";
import { Input } from "../ui/input";
import { Label } from "../ui/label";
import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
    CardFooter
} from "@/components/ui/card"
import Loader from "../common/loader";
import ReservationCancellationAlertDialog from "./reservation-cancellation-alert-dialog"
import { Textarea } from "../ui/textarea";
import { Checkbox } from "../ui/checkbox";
import Error from "../common/error";

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
            <div className="flex flex-1 flex-col gap-4 p-4 md:gap-8 md:p-8">
                <div className="grid gap-4 md:gap-8">
                    <Card x-chunk="dashboard-01-chunk-0">
                        <CardHeader className="flex flex-row items-center justify-center space-y-0 pb-4 p-0 lg:p-4 xl:p-6">
                            <CardTitle className="text-xl font-medium">
                                Reservation Details
                            </CardTitle>
                        </CardHeader>
                        <CardContent className="flex flex-col items-center justify-center text-center pb-4">
                            <div className="w-full lg:w-auto grid gap-4 py-4">
                                <div className="flex lg:flex-none flex-col lg:grid lg:grid-cols-12 lg:items-center gap-4">
                                    <Label htmlFor="name" className="text-left lg:text-right col-span-2">
                                        Name
                                    </Label>
                                    <Input
                                        id="name"
                                        value={data.data.firstName + ' ' + data.data.lastName}
                                        className="col-span-10"
                                        disabled
                                    />
                                </div>
                                <div className="flex lg:flex-none flex-col lg:grid lg:grid-cols-12 lg:items-center gap-4">
                                    <Label htmlFor="phoneNumber" className="text-left lg:text-right col-span-2">
                                        Phone Number
                                    </Label>
                                    <Input
                                        id="phoneNumber"
                                        value={data.data.phoneNumber}
                                        className="col-span-10"
                                        disabled
                                    />
                                </div>
                                <div className="flex lg:flex-none flex-col lg:grid lg:grid-cols-12 lg:items-center gap-4">
                                    <Label htmlFor="note" className="text-left lg:text-right col-span-2">
                                        Note
                                    </Label>
                                    <Textarea
                                        id="note"
                                        value={data.data.note}
                                        className="col-span-10"
                                        disabled
                                    />
                                </div>
                                <div className="flex lg:flex-none flex-col lg:grid lg:grid-cols-12 lg:items-center gap-4">
                                    <Label htmlFor="guestsArrivingAt" className="text-left lg:text-right col-span-2">
                                        Date and Time
                                    </Label>
                                    <Input
                                        id="guestsArrivingAt"
                                        value={data.data.guestsArrivingAt}
                                        className="col-span-10"
                                        disabled
                                    />
                                </div>
                                <div className="flex lg:flex-none flex-col lg:grid lg:grid-cols-12 lg:items-center gap-4">
                                    <Label htmlFor="numberOfGuests" className="text-left lg:text-right col-span-2">
                                        Number Of Guests
                                    </Label>
                                    <Input
                                        id="numberOfGuests"
                                        value={data.data.numberOfGuests}
                                        className="col-span-10"
                                        disabled
                                    />
                                </div>
                                <div className="flex lg:flex-none flex-col lg:grid lg:grid-cols-12 lg:items-center gap-4">
                                    <Label htmlFor="locationName" className="text-left lg:text-right col-span-2">
                                        Location
                                    </Label>
                                    <Input
                                        id="locationName"
                                        value={data.data.locationName}
                                        className="col-span-10"
                                        disabled
                                    />
                                </div>
                                <div className="flex lg:flex-none flex-col lg:grid lg:grid-cols-12 lg:items-center gap-4">
                                    <Label htmlFor="tableName" className="text-left lg:text-right col-span-2">
                                        Table
                                    </Label>
                                    <Input
                                        id="tableName"
                                        value={data.data.tableName}
                                        className="col-span-10"
                                        disabled
                                    />
                                </div>
                                <div className="flex flex-row items-center justify-center gap-4">
                                    <Label htmlFor="isConfirmed" className="text-right">
                                        Is the Reservation Confirmed?
                                    </Label>
                                    <Checkbox
                                        id="isConfirmed"
                                        value={data.data.isConfirmed}
                                        className=""
                                        disabled
                                    />
                                </div>
                            </div>
                        </CardContent>
                        <CardFooter className="flex flex-col items-center justify-center">
                            <div className="flex flex-row justify-center gap-4">
                                {data.data.canBeCancelled ?
                                    <ReservationCancellationAlertDialog props={data.data} />
                                    : 
                                    <p className="text-md text-center text-slate-500 pt-4 w-2/3">
                                        Unfortunately the Reservation has already been confirmed and you can no longer cancel it through out platform. Please constact <strong>{data.data.locationName}</strong> via email at <strong>{data.data.locationEmail}</strong>
                                    </p>
                                }
                            </div>
                        </CardFooter>
                    </Card>
                </div>
            </div>
        )
    } else if (isError) {
        content = <Error />;
    }

    return content
}

export default ReservationDetailsForm