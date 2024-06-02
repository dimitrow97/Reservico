import ReservationDetailsForm from "../components/reservations/reservation-details-form"
import { useParams } from "react-router-dom";

const ReservationDetails = () => {
    const { id } = useParams()

    return (
        <ReservationDetailsForm id={id} />
    )
}

export default ReservationDetails