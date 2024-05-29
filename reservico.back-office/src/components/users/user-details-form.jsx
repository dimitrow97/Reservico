import { useGetUserQuery } from "../../features/users/users-api-slice"
import Loader from "../common/loader";
import { Input } from "../ui/input";
import { Label } from "../ui/label";
import UserEditDialog from "./user-edit-dialog";

const UserDetailsForm = (props) => {
    const {
        data,
        error,
        isError,
        isLoading,
        isSuccess } = useGetUserQuery(props.id)

    let content;
    if (isLoading) {
        content = <Loader />;
    } else if (isSuccess) {    
        content = (
            <div>
                <div className="grid gap-4 py-4">
                    <div className="grid grid-cols-12 items-center gap-4">
                        <Label htmlFor="id" className="text-right col-span-2">
                            Id
                        </Label>
                        <Input
                            id="id"
                            value={data.data.userId}
                            className="col-span-10"
                            disabled
                        />
                    </div>
                    <div className="grid grid-cols-12 items-center gap-4">
                        <Label htmlFor="name" className="text-right col-span-2">
                            Name
                        </Label>
                        <Input
                            id="name"
                            value={data.data.firstName + ' ' + data.data.lastName}
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
                            value={data.data.email}
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
                            value={data.data.phoneNumber}
                            className="col-span-10"
                            disabled
                        />
                    </div>   
                </div>
                <div className="flex flex-row justify-center gap-4">  
                    <UserEditDialog props={data.data} />
                </div>
            </div>
        )
    } else if (isError) {
        content = <p>{JSON.stringify(error)}</p>;
    }

    return content
}

export default UserDetailsForm