import AccountProfileTabs from "../components/account/account-profile-tabs"

import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
} from "@/components/ui/card"

const AccountProfile = () => {

    return (
        <Card className="w-2/3 h-full">
            <CardHeader>
                <CardTitle>Account Profile</CardTitle>
            </CardHeader>
            <CardContent className="grid gap-4">
                <AccountProfileTabs />
            </CardContent>
        </Card>
    )
}

export default AccountProfile