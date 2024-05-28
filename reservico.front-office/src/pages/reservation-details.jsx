import ReservationDetailsForm from "../components/reservations/reservation-details-form"
import { useLocation } from 'react-router-dom';

import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
} from "@/components/ui/card"

const ReservationDetails = () => {
    const location = useLocation();

    return (
        <div className="grid grid-cols-2 gap-4">
            <Card className="h-full">
                <CardHeader>
                    <CardTitle>Reservation Details</CardTitle>
                </CardHeader>
                <CardContent className="grid gap-4">
                    <ReservationDetailsForm id={location.state.reservationId} />
                </CardContent>
            </Card>            
        </div>
    )
}

export default ReservationDetails