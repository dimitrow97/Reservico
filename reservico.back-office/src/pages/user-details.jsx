import UserDetailsForm from "../components/users/user-details-form"
import UserClients from "../components/users/user-clients";
import { useLocation } from 'react-router-dom';

import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
} from "@/components/ui/card"

const UserDetails = () => {
    const location = useLocation();

    return (
        <div className="grid grid-cols-2 gap-4">
            <Card className="h-full">
                <CardHeader>
                    <CardTitle>User Details</CardTitle>
                </CardHeader>
                <CardContent className="grid gap-4">
                    <UserDetailsForm id={location.state.userId} />
                </CardContent>
            </Card>
            <Card className="h-full">
                <CardHeader>
                    <CardTitle>User Clients</CardTitle>
                </CardHeader>
                <CardContent className="grid gap-4">
                    <UserClients id={location.state.userId} />
                </CardContent>
            </Card>
        </div>
    )
}

export default UserDetails