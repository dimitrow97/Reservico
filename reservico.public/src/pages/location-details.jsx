import Error from "@/components/common/error";
import Loader from "@/components/common/loader"
import LocationDetailsComp from "@/components/locations/location-details-comp";
import ReservationForm from "@/components/reservations/reservation-form";
import { useGetLocationQuery } from "@/features/locations/locations-api-slice";
import { useParams } from "react-router-dom";

const LocationDetails = () => {
    const { id } = useParams()

    const {
        data: location,
        isLoading,
        isSuccess,
        isError,
        error
    } = useGetLocationQuery(id)

    let content;
    if (isLoading) {
        content = <Loader />;
    } else if (isSuccess) {
        content = (
            <div className="flex flex-1 flex-col gap-4 p-4 md:gap-8 md:p-8">
                <div className="grid gap-4 md:grid-cols-2 md:gap-8 lg:grid-cols-2">
                    <LocationDetailsComp location={location.data} />
                    <ReservationForm 
                        locationId={id} 
                        workingHoursFrom={location.data.workingHoursFrom}
                        workingHoursTo={location.data.workingHoursTo}
                     />
                </div>
            </div>
        )
    } else if (isError) {
        content = <Error />;
    }

    return content
}

export default LocationDetails