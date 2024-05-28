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
import { useToast } from "@/components/ui/use-toast"
import { useState } from 'react'
import { useChangePasswordMutation } from "../../features/users/users-api-slice"

const AccountProfileChangePassword = () => {
    const { toast } = useToast()
    const [pwd, setPwd] = useState("")    
    const [newPwd, setNewPwd] = useState("")

    const handlePwdInput = (e) => setPwd(e.target.value)
    const handleNewPwdInput = (e) => setNewPwd(e.target.value)

    const [changePassword] = useChangePasswordMutation();

    const handleSubmit = async (e) => {
        e.preventDefault()

        const changePwdRequestModel = {
            password: pwd,
            newPassword: newPwd,
        }

        try {
            const response = await changePassword(changePwdRequestModel).unwrap()

            if (response.isSuccess) {
                toast({
                    title: "Password changed!",
                    description: "You have successfully changed your password!",
                })
            }
            else {
                toast({
                    title: "Changing your password was unsuccessfull!",
                    description: response.errorMessage,
                })
            }
        } catch (err) {
            console.log(err);
        }
    };

    return (
        <Card>
            <CardHeader>
                <CardTitle>Password</CardTitle>
                <CardDescription>
                    Change your password here. After saving, you'll be logged out.
                </CardDescription>
            </CardHeader>
            <CardContent className="space-y-2">
                <div className="space-y-1">
                    <Label htmlFor="current">Current password</Label>
                    <Input id="current" value={pwd} onChange={handlePwdInput} type="password" required />
                </div>
                <div className="space-y-1">
                    <Label htmlFor="new">New password</Label>
                    <Input id="new" value={newPwd} onChange={handleNewPwdInput} type="password" required />
                </div>
            </CardContent>
            <CardFooter>
                <Button onClick={handleSubmit}>Save password</Button>
            </CardFooter>
        </Card>
    )
}

export default AccountProfileChangePassword