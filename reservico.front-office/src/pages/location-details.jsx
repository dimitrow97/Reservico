import LocationDetailsForm from "../components/locations/location-details-form"
import LocationTables from "../components/locations/tables/location-tables"
import { useLocation } from 'react-router-dom'

import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
} from "@/components/ui/card"

const LocationDetails = () => {
    const location = useLocation();

    return (
        <div className="grid gap-4">
            <Card className="h-full">
                <CardHeader>
                    <CardTitle>Location Details</CardTitle>
                </CardHeader>
                <CardContent className="grid gap-4">
                    <LocationDetailsForm id={location.state.locationId} />
                </CardContent>
            </Card>     
            <Card className="h-full">
                <CardHeader>
                    <CardTitle>Location Tables</CardTitle>
                </CardHeader>
                <CardContent className="grid gap-4">
                    <LocationTables id={location.state.locationId} />
                </CardContent>
            </Card>       
        </div>
    )
}

export default LocationDetails