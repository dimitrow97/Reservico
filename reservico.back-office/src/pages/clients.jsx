import ClientsDataTable from "../components/clients/clients-data-table"
import { useGetClientsQuery } from "../features/clients/clients-api-slice"

import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
  } from "@/components/ui/card"

const Clients = () => {
    const {
        data: clients,
        isLoading,
        isSuccess,
        isError,
        error
    } = useGetClientsQuery()

    let content;
    if (isLoading) {
        content = <p>"Loading..."</p>;
    } else if (isSuccess) {
        content = (
            <Card className="w-full h-full">
                <CardHeader>
                    <CardTitle>Clients</CardTitle>
                </CardHeader>
                <CardContent className="grid gap-4">
                    <ClientsDataTable data={clients.data} />
                </CardContent>
            </Card>
        )
    } else if (isError) {
        content = <p>{JSON.stringify(error)}</p>;
    }

    return content
}

export default Clients