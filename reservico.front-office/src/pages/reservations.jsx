import ReservationsDataTable from "../components/reservations/reservations-data-table"
import { useGetReservationsQuery } from "../features/reservations/reservations-api-slice"
import { useSelector } from "react-redux"
import { selectCurrentClient } from "../features/auth/auth-slice"
import Loader from "../components/common/loader"

import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
  } from "@/components/ui/card"

const Reservations = () => {
    const currentClient = useSelector(selectCurrentClient)

    const {
        data: reservations,
        isLoading,
        isSuccess,
        isError,
        error
    } = useGetReservationsQuery(currentClient)

    let content;
    if (isLoading) {
        content = <Loader />;
    } else if (isSuccess) {
        content = (
            <Card className="w-full h-full">
                <CardHeader>
                    <CardTitle>Reservations</CardTitle>
                </CardHeader>
                <CardContent className="grid gap-4">
                    <ReservationsDataTable data={reservations.data} />
                </CardContent>
            </Card>
        )
    } else if (isError) {
        content = <p>{JSON.stringify(error)}</p>;
    }

    return content
}

export default Reservations