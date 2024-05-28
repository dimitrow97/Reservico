import LocationTableDetailsForm from "../components/locations/tables/location-table-details-form"
import { useLocation } from 'react-router-dom'

import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
} from "@/components/ui/card"

const TableDetails = () => {
    const location = useLocation();

    return (
        <div className="grid gap-4">
            <Card className="h-full">
                <CardHeader>
                    <CardTitle>Location Table Details</CardTitle>
                </CardHeader>
                <CardContent className="grid gap-4">
                    <LocationTableDetailsForm id={location.state.tableId} />
                </CardContent>
            </Card>          
        </div>
    )
}

export default TableDetails