import * as React from "react"
import Image from "@/components/ui/image"
import { useGetLocationImagesQuery } from "../../../features/locations/images/images-api-slice"
import { ScrollArea, ScrollBar } from "@/components/ui/scroll-area"
import Loader from "../../common/loader"
import LocationImageDeleteAlertDialog from "./location-image-delete-alert-dialog"

const LocationImagesScroll = ({ locationId }) => {
    const srcPrefix = "https://resstgaccmpxdkxmudev.blob.core.windows.net/"
    const {
        data: images,
        error,
        isError,
        isLoading,
        isSuccess } = useGetLocationImagesQuery(locationId)

    let content;
    if (isLoading) {
        content = <Loader />;
    } else if (isSuccess) {
        content = (
            <ScrollArea className="w-full whitespace-nowrap rounded-md border">
                <div className="flex w-max space-x-4 p-4">
                    {images.data.locationImages.map((image, key) => (
                        <figure key={key} className="shrink-0 w-max-[450px] h-max-[450px]">
                            <div className="overflow-hidden rounded-md">
                                <Image
                                    src={srcPrefix + image.blobPath}
                                    alt={`${image.locationImageId}`}
                                    className="aspect-[3/4] object-cover"
                                    width={450}
                                    height={450}
                                />
                            </div>
                            <figcaption className="flex flex-col items-center pt-2 text-xs text-muted-foreground">
                                <LocationImageDeleteAlertDialog locationImageId={image.locationImageId} />
                            </figcaption>
                        </figure>
                    ))}
                </div>
                <ScrollBar orientation="horizontal" />
            </ScrollArea>
        )
    } else if (isError) {
        content = <p>{JSON.stringify(error)}</p>;
    }

    return content
}

export default LocationImagesScroll