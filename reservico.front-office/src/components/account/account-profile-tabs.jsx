import {
    Tabs,
    TabsContent,
    TabsList,
    TabsTrigger,
} from "@/components/ui/tabs"
import AccountProfileDetails from "./account-profile-details"
import AccountProfileChangePassword from "./account-profile-change-password"

export function AccountProfileTabs() {

    return (
        <Tabs defaultValue="account">
            <TabsList className="grid w-full grid-cols-2">
                <TabsTrigger value="account">Account</TabsTrigger>
                <TabsTrigger value="password">Change Password</TabsTrigger>
            </TabsList>
            <TabsContent value="account">
                <AccountProfileDetails />
            </TabsContent>
            <TabsContent value="password">
                <AccountProfileChangePassword />
            </TabsContent>
        </Tabs>
    )
}

export default AccountProfileTabs