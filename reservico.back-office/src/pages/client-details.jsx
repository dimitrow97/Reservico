import ClientDetailsForm from "../components/clients/client-details-form"
import ClientUsers from "../components/clients/client-users";
import { useLocation } from 'react-router-dom';

import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
} from "@/components/ui/card"

const ClientDetails = () => {
    const location = useLocation();

    return (
        <div className="grid grid-cols-2 gap-4">
            <Card className="h-full">
                <CardHeader>
                    <CardTitle>Client Details</CardTitle>
                </CardHeader>
                <CardContent className="grid gap-4">
                    <ClientDetailsForm id={location.state.id} />
                </CardContent>
            </Card>
            <Card className="h-full">
                <CardHeader>
                    <CardTitle>Client Users</CardTitle>
                </CardHeader>
                <CardContent className="grid gap-4">
                    <ClientUsers id={location.state.id} />
                </CardContent>
            </Card>
        </div>
    )
}

export default ClientDetails