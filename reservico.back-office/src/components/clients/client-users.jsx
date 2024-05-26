import ClientUsersDataTable from "./client-users-data-table";
import { useGetUsersForClientQuery } from "../../features/users/users-api-slice"
import Loader from "../common/loader";

const ClientUsers = (props) => {
    const {
        data: users,
        error,
        isError,
        isLoading,
        isSuccess } = useGetUsersForClientQuery(props.id)    

    let content;
    if (isLoading) {
        content = (
            <Loader />
        )
    } else if (isSuccess) { 
        content = (
            <ClientUsersDataTable data={users.data} />
        )
    } else if (isError) {
        content = <p>{JSON.stringify(error)}</p>;
    }

    return content
}

export default ClientUsers