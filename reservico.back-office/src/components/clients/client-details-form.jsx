import { useGetClientDetailsQuery } from "../../features/clients/clients-api-slice"
import { Input } from "../ui/input";
import { Label } from "../ui/label";
import ClientEditDialog from "./client-edit-dialog";
import ClientDeleteAlertDialog from "./client-delete-alert-dialog"
import Loader from "../common/loader";

const ClientDetailsForm = (props) => {
    const {
        data,
        error,
        isError,
        isLoading,
        isSuccess } = useGetClientDetailsQuery(props.id)

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
                            value={data.data.id}
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
                            value={data.data.name}
                            className="col-span-10"
                            disabled
                        />
                    </div>
                    <div className="grid grid-cols-12 items-center gap-4">
                        <Label htmlFor="address" className="text-right col-span-2">
                            Address
                        </Label>
                        <Input
                            id="address"
                            value={data.data.address}
                            className="col-span-10"
                            disabled
                        />
                    </div>
                    <div className="grid grid-cols-12 items-center gap-4">
                        <Label htmlFor="city" className="text-right col-span-2">
                            City
                        </Label>
                        <Input
                            id="city"
                            value={data.data.city}
                            className="col-span-10"
                            disabled
                        />
                    </div>
                    <div className="grid grid-cols-12 items-center gap-4">
                        <Label htmlFor="postcode" className="text-right col-span-2">
                            Post Code
                        </Label>
                        <Input
                            id="postcode"
                            value={data.data.postcode}
                            className="col-span-10"
                            disabled
                        />
                    </div>
                    <div className="grid grid-cols-12 items-center gap-4">
                        <Label htmlFor="country" className="text-right col-span-2">
                            Country
                        </Label>
                        <Input
                            id="country"
                            value={data.data.country}
                            className="col-span-10"
                            disabled
                        />
                    </div>
                </div>
                <div className="flex flex-row justify-center gap-4">                    
                    <ClientEditDialog props={data.data} />
                    <ClientDeleteAlertDialog props={data.data} />
                </div>
            </div>
        )
    } else if (isError) {
        content = <p>{JSON.stringify(error)}</p>;
    }

    return content
}

export default ClientDetailsForm