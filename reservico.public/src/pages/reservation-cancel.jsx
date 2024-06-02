import ReservationCancelAlertDialog from "@/components/reservations/reservation-cancel-alert-dialog";
import { useParams } from "react-router-dom";
import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
    CardFooter
} from "@/components/ui/card"

const ReservationCancel = () => {
    const { id } = useParams()

    return (
        <div className="flex flex-1 flex-col gap-4 p-4 md:gap-8 md:p-8">
            <div className="grid gap-4 md:gap-8">
                <Card x-chunk="dashboard-01-chunk-0">
                    <CardHeader className="flex flex-row items-center justify-center space-y-0 pb-4 p-0 lg:p-4 xl:p-6">
                        <CardTitle className="text-xl font-medium">
                            Reservation Cancellation
                        </CardTitle>
                    </CardHeader>
                    <CardContent className="flex flex-col items-center justify-center text-center pb-4">
                        <ReservationCancelAlertDialog id={id} />
                    </CardContent>
                </Card>
            </div>
        </div>
    )
}

export default ReservationCancel