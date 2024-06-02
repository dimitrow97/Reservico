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
import {
    Pagination,
    PaginationContent,
    PaginationItem,
    PaginationLink,
    PaginationNext,
    PaginationPrevious,
} from "@/components/ui/pagination"
import { Badge } from "@/components/ui/badge"
import Loader from "@/components/common/loader"
import { useState } from 'react'
import { useGetLocationsQuery } from "@/features/locations/locations-api-slice"
import { Button } from "../ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import CategoriesMultiSelect from "../categories/categories-multi-select"
import Error from "../common/error"

const LocationsData = ({ categories }) => {
    const srcPrefix = "https://resstgaccmpxdkxmudev.blob.core.windows.net/"

    const [filter, setFilter] = useState("")
    const [nameFilter, setNameFilter] = useState("")
    const [cityAndCountryFilter, setCityAndCountryFilter] = useState("")
    const [categoryFilter, setCategoryFilter] = useState([])
    const [page, setPage] = useState(0)
    const take = 9;

    const handleNameFilter = (e) => setNameFilter(e.target.value)
    const handleCityAndCountryFilter = (e) => setCityAndCountryFilter(e.target.value)

    const applyFilers = () => {
        let currentFilter = ""
        console.log(nameFilter)
        if (nameFilter !== "") {
            currentFilter += "name.contains(\"" + nameFilter + "\")"
        }
        if (cityAndCountryFilter !== "") {
            let and = ""

            if (currentFilter !== "") {
                and = " and "
            }
            currentFilter += and + "(city.contains(\"" + cityAndCountryFilter + "\") or country.contains(\"" + cityAndCountryFilter + "\"))"
        }
        if (categoryFilter.length > 0) {
            let and = ""

            if (currentFilter !== "") {
                and = " and "
            }

            for (let i = 0; i < categoryFilter.length; i++) {
                if (i !== 0) and = " and "

                currentFilter += and + "category.contains(\"" + categoryFilter[i].name + "\")"
            }
        }

        setFilter(currentFilter)
    }

    const clearFilters = () => {
        setNameFilter("")
        setCityAndCountryFilter("")
        setCategoryFilter([])
        setFilter("")
    }

    const {
        data: locations,
        isLoading,
        isSuccess,
        isError,
        error
    } = useGetLocationsQuery({ filter: filter, skip: page * take, take: take })

    let content;
    if (isLoading) {
        content = <Loader />;
    } else if (isSuccess) {
        content = (
            <div>
                <div className="flex flex-1 flex-col gap-4 p-4 md:gap-8 md:p-8">
                    <div className="grid md:grid-cols-4 md:gap-4 lg:grid-cols-4">
                        <Card x-chunk="dashboard-01-chunk-2" className="border-0 shadow-none bg-transparent">
                            <CardContent className="flex flex-col items-center justify-center lg:ml-auto text-center">
                                <div className="grid w-full items-center gap-1">
                                    <Label htmlFor="nameFilter" className="text-left">
                                        Search by name...
                                    </Label>
                                    <Input
                                        id="nameFilter"
                                        placeholder={"Simply search by name..."}
                                        onChange={handleNameFilter}
                                        value={nameFilter}
                                        required
                                    />
                                </div>
                            </CardContent>
                        </Card>
                        <Card x-chunk="dashboard-01-chunk-2" className="border-0 shadow-none bg-transparent">
                            <CardContent className="flex flex-col items-center justify-center text-center">
                                <div className="grid w-full items-center gap-1">
                                    <Label htmlFor="categories" className="text-left">
                                        Search by categories...
                                    </Label>
                                    <CategoriesMultiSelect
                                        categories={categories.data}
                                        selected={categoryFilter}
                                        setSelected={setCategoryFilter}
                                    />
                                </div>
                            </CardContent>
                        </Card>
                        <Card x-chunk="dashboard-01-chunk-2" className="border-0 shadow-none bg-transparent">
                            <CardContent className="flex flex-col items-center justify-center text-center">
                                <div className="grid w-full items-center gap-1">
                                    <Label htmlFor="categories" className="text-left">
                                        Search by City or Country...
                                    </Label>
                                    <Input
                                        id="cityAndCountryFilter"
                                        placeholder={"Simply search by City or Country..."}
                                        onChange={handleCityAndCountryFilter}
                                        value={cityAndCountryFilter}
                                        required
                                    />
                                </div>
                            </CardContent>
                        </Card>
                        <Card x-chunk="dashboard-01-chunk-2" className="border-0 shadow-none bg-transparent">
                            <CardContent className="flex flex-col items-center justify-center lg:ml-auto text-center">
                                <div className="grid w-full items-left gap-1">
                                    <Label htmlFor="categories" className="text-left">
                                        Don`t worry about it... Just...
                                    </Label>
                                    <div className="flex flex-row gap-2">
                                        <Button onClick={applyFilers} className="w-3/4 bg-[#28282a] hover:border-2 hover:bg-white hover:text-black">
                                            <span>Apply the Filters</span>
                                        </Button>
                                        <Button onClick={clearFilters} className="w-1/4 bg-[#28282a] hover:border-2 hover:bg-white hover:text-black">
                                            <span>Clear</span>
                                        </Button>
                                    </div>
                                </div>
                            </CardContent>
                        </Card>
                    </div>
                </div>
                <div className="flex flex-1 flex-col gap-4 p-4 md:gap-8 md:p-8">
                    <div className="grid gap-4 md:grid-cols-3 md:gap-8 lg:grid-cols-3">
                        {locations.data.data.map((location, key) => (
                            <Card key={key} x-chunk="dashboard-01-chunk-0">
                                <CardHeader className="flex flex-row items-center justify-center space-y-0 pb-4 p-0 lg:p-4 xl:p-6">
                                    <CardTitle className="text-xl font-medium">
                                        {location.name}
                                    </CardTitle>
                                </CardHeader>
                                <CardContent className="flex flex-col items-center justify-center text-center pb-4">
                                    <div className="text-2xl font-bold">
                                        <Carousel className="w-full max-w-64 lg:max-w-80">
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
                                    <div className="flex flex-row text-m text-muted-foreground gap-1 pb-2 w-full items-center justify-center">
                                        {location.categories.map((cat, key) => (
                                            <Badge key={key} variant="secondary">{cat.name}</Badge>
                                        ))}
                                    </div>
                                    <div className="flex flex-row text-m text-muted-foreground gap-1 pb-2 w-3/4">
                                        <MapPinned />
                                        <span>{location.address}, {location.city}</span>
                                    </div>
                                    <div className="grid grid-cols-2 gap-4 w-3/4">
                                        <div className="flex flex-row text-m text-muted-foreground gap-1">
                                            <MapPin />
                                            <span>{location.country}</span>
                                        </div>
                                        <div className="flex flex-row text-m text-muted-foreground gap-1 ml-auto pr-2">
                                            <Clock4 />
                                            <span>{location.workingHoursFrom}:00-{location.workingHoursTo}:00</span>
                                        </div>
                                    </div>
                                    <div className="flex flex-row text-m text-muted-foreground gap-1 pt-4 w-full items-center justify-center">
                                        <Button
                                            onClick={() => {
                                                window.location.href = '/location-details/' + location.locationId
                                            }}
                                            className="bg-[#28282a]">
                                            <span>Detils and Reservation</span>
                                        </Button>
                                    </div>
                                </CardFooter>
                            </Card>
                        ))}
                    </div>
                </div>
                <div className="flex flex-row text-m text-muted-foreground gap-1 pb-8 w-full items-center justify-center">
                    <Pagination>
                        <PaginationContent>
                            <PaginationItem>
                                <PaginationPrevious
                                    onClick={() => {
                                        if (page > 0) {
                                            setPage(page => page - 1)
                                        }
                                    }}
                                />
                            </PaginationItem>
                            <PaginationItem>
                                <PaginationLink className="w-24" disabled isActive>
                                    {page + 1} out of {locations.data.numberOfPages}
                                </PaginationLink>
                            </PaginationItem>
                            <PaginationItem>
                                <PaginationNext
                                    onClick={() => {
                                        if (page < locations.data.numberOfPages - 1) {
                                            setPage(page => page + 1)
                                        }
                                    }}
                                />
                            </PaginationItem>
                        </PaginationContent>
                    </Pagination>
                </div>
            </div>
        )
    } else if (isError) {
        content = <Error />;
    }

    return content
}

export default LocationsData