import { Button } from "@/components/ui/button"
import {
    Card,
    CardContent,
    CardDescription,
    CardFooter,
    CardHeader,
    CardTitle,
} from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { useSelector } from 'react-redux'
import { selectCurrentUserData } from "../../features/auth/auth-slice"

const AccountProfileDetails = () => {
    const userData = useSelector(selectCurrentUserData)

    return (
        <div>
            <Card>
                <CardHeader>
                    <CardTitle>Account</CardTitle>
                </CardHeader>
                <CardContent className="space-y-2">
                    <div className="grid gap-4 py-4">
                        <div className="grid grid-cols-12 items-center gap-4">
                            <Label htmlFor="id" className="text-right col-span-2">
                                Id
                            </Label>
                            <Input
                                id="id"
                                value={userData.userId}
                                className="col-span-10"
                                disabled
                            />
                        </div>
                        <div className="grid grid-cols-12 items-center gap-4">
                            <Label htmlFor="firstName" className="text-right col-span-2">
                                First Name
                            </Label>
                            <Input
                                id="firstName"
                                value={userData.firstName}
                                className="col-span-10"
                                disabled
                            />
                        </div>
                        <div className="grid grid-cols-12 items-center gap-4">
                            <Label htmlFor="lastName" className="text-right col-span-2">
                                Last Name
                            </Label>
                            <Input
                                id="lastName"
                                value={userData.lastName}
                                className="col-span-10"
                                disabled
                            />
                        </div>
                        <div className="grid grid-cols-12 items-center gap-4">
                            <Label htmlFor="email" className="text-right col-span-2">
                                Email
                            </Label>
                            <Input
                                id="email"
                                value={userData.email}
                                className="col-span-10"
                                disabled
                            />
                        </div>
                        <div className="grid grid-cols-12 items-center gap-4">
                            <Label htmlFor="phoneNumber" className="text-right col-span-2">
                                Phone Number
                            </Label>
                            <Input
                                id="phoneNumber"
                                value={userData.phoneNumber}
                                className="col-span-10"
                                disabled
                            />
                        </div>
                    </div>
                </CardContent>
                <CardFooter>
                </CardFooter>
            </Card>
        </div>

    )
}

export default AccountProfileDetails