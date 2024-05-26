import Loader from "../components/common/loader"
import UsersDataTable from "../components/users/users-data-table"
import { useGetUsersQuery } from "../features/users/users-api-slice"

import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
  } from "@/components/ui/card"

const Users = () => {
    const {
        data: users,
        isLoading,
        isSuccess,
        isError,
        error
    } = useGetUsersQuery()

    let content;
    if (isLoading) {
        content = <Loader />;
    } else if (isSuccess) {
        content = (
            <Card className="w-full h-full">
                <CardHeader>
                    <CardTitle>Users</CardTitle>
                </CardHeader>
                <CardContent className="grid gap-4">
                    <UsersDataTable data={users.data.data} />
                </CardContent>
            </Card>
        )
    } else if (isError) {
        content = <p>{JSON.stringify(error)}</p>;
    }    

    return content
}

export default Users