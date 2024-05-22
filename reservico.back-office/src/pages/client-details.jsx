import ClientDetailsForm from "../components/clients/client-details-form"
import {useLocation} from 'react-router-dom';

import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
} from "@/components/ui/card"

const ClientDetails = () => {
    const location = useLocation(); 

    return (
        <Card className="w-1/2 h-full">
            <CardHeader>
                <CardTitle>Client Details</CardTitle>
            </CardHeader>
            <CardContent className="grid gap-4">
                <ClientDetailsForm id={location.state.id}/>
            </CardContent>
        </Card>
    )
}

export default ClientDetails