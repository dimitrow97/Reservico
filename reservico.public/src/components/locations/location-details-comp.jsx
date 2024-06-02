import {
    Clock4,
    MapPin,
    MapPinned
} from "lucide-react"
import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
    CardFooter
} from "@/components/ui/card"
import {
    Carousel,
    CarouselContent,
    CarouselItem,
    CarouselNext,
    CarouselPrevious,
} from "@/components/ui/carousel"
import { Badge } from "@/components/ui/badge"

const LocationDetailsComp = ({ location }) => {
    const srcPrefix = "https://resstgaccmpxdkxmudev.blob.core.windows.net/"
    
    return (
        <Card x-chunk="dashboard-01-chunk-0">
            <CardHeader className="flex flex-row items-center justify-center space-y-0 pb-8 p-0 lg:p-4 xl:p-6">
                <CardTitle className="text-xl font-medium">
                    {location.name}
                </CardTitle>
            </CardHeader>
            <CardContent className="flex flex-col items-center justify-center text-center pb-8">
                <div className="text-2xl font-bold">
                    <Carousel className="w-full max-w-64 lg:max-w-md">
                        <CarouselContent>
                            {location.locationImages && location.locationImages.length ? (
                                location.locationImages.map((image, index) => (
                                    <CarouselItem key={index}>
                                        <div className="p-1">
                                            <Card>
                                                <CardContent className="overflow-hidden flex aspect-square items-center justify-center p-0">
                                                    <img
                                                        src={srcPrefix + image.blobPath}
                                                        alt={`${image.locationImageId}`}
                                                        className="aspect-[3/4] object-cover"
                                                    />
                                                </CardContent>
                                            </Card>
                                        </div>
                                    </CarouselItem>
                                ))
                            ) :
                                (
                                    <CarouselItem>
                                        <div className="p-1">
                                            <Card>
                                                <CardContent className="overflow-hidden flex aspect-square items-center justify-center p-0">
                                                    <img
                                                        src="/no-image.jpg"
                                                        alt="default"
                                                        className="aspect-[3/4] object-cover"
                                                    />
                                                </CardContent>
                                            </Card>
                                        </div>
                                    </CarouselItem>
                                )}
                        </CarouselContent>
                        <CarouselPrevious />
                        <CarouselNext />
                    </Carousel>
                </div>
            </CardContent>
            <CardFooter className="flex flex-col items-center justify-center">
                <div className="flex flex-row text-m text-muted-foreground gap-1 pb-8 w-full items-center justify-center">
                    {location.categories.map((cat, key) => (
                        <Badge key={key} variant="secondary">{cat.name}</Badge>
                    ))}
                </div>

                <div className="gap-4 w-3/4">
                    <div className="flex flex-row text-m text-muted-foreground gap-2 pb-4">
                        <MapPinned />
                        <span>{location.address}, {location.city}</span>
                    </div>
                    <div className="flex flex-row text-m text-muted-foreground gap-2 pb-4">
                        <Clock4 />
                        <span>{location.workingHoursFrom}:00-{location.workingHoursTo}:00</span>
                    </div>
                    <div className="flex flex-row text-m text-muted-foreground gap-2 pb-4">
                        <MapPin />
                        <span>{location.country}</span>
                    </div>
                </div>
            </CardFooter>
        </Card>
    )
}

export default LocationDetailsComp