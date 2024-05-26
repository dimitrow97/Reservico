import UserClientsDataTable from "./user-clients-data-table";
import { useGetClientsForUserQuery } from "../../features/users/users-api-slice"
import Loader from "../common/loader";

const UserClients = (props) => {
    const {
        data: userClients,
        error,
        isError,
        isLoading,
        isSuccess } = useGetClientsForUserQuery(props.id)    

    let content;
    if (isLoading) {
        content = (
            <Loader />
        )
    } else if (isSuccess) { 
        content = (
            <UserClientsDataTable data={userClients.data} userId={props.id} />
        )
    } else if (isError) {
        content = <p>{JSON.stringify(error)}</p>;
    }

    return content
}

export default UserClients