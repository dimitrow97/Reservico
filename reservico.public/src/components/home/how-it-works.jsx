import {
    FastForward,
    CalendarClock,
    MapPin
} from "lucide-react"
import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
} from "@/components/ui/card"
import { Separator } from "@/components/ui/separator"

const HowItWorks = () => {
    return (
        <div className="h-auto xl:h-[630px] bg-[url('/background.jpeg')] bg-center bg-cover overflow-hidden">
            <div className="p-4 items-center justify-center">
                <div className="w-full rounded-lg p-4 text-white flex flex-col items-center justify-center">
                    <span className="text-4xl font-bold pb-2 drop-shadow-[0_1.2px_1.2px_rgba(0,0,0,0.8)]">
                        How it works?
                    </span>
                    <Separator className="w-1/6 h-1" />
                </div>
                <div className="flex flex-1 flex-col gap-4 p-4 md:gap-8 md:p-8">
                    <div className="grid gap-4 md:grid-cols-3 md:gap-8 lg:grid-cols-3">
                        <Card x-chunk="dashboard-01-chunk-2" className="border-0 shadow-none bg-transparent text-white">
                            <CardHeader className="flex flex-row items-center justify-center space-y-0 pb-8">
                                <CardTitle className="text-sm font-medium">
                                    <MapPin color="#ffffff" className="h-12 w-12 text-muted-foreground" />
                                </CardTitle>
                            </CardHeader>
                            <CardContent className="flex flex-col items-center justify-center text-center">
                                <div className="text-2xl font-bold pb-4">1. Choose a Location</div>
                                <p className="text-m w-3/4">
                                Explore our curated collection of Locations, restaurants, clubs, cafes, and bars! Whether you seek culinary delights, vibrant nightlife, or cozy spots to unwind, our catalog has it all. Let us guide you to your next unforgettable experience. Your journey begins here.
                                </p>
                            </CardContent>
                        </Card>
                        <Card x-chunk="dashboard-01-chunk-0" className="border-0 shadow-none bg-transparent text-white">
                            <CardHeader className="flex flex-row items-center justify-center space-y-0 pb-8">
                                <CardTitle className="text-sm font-medium">
                                    <CalendarClock color="#ffffff" className="h-12 w-12 text-muted-foreground" />
                                </CardTitle>
                            </CardHeader>
                            <CardContent className="flex flex-col items-center justify-center text-center">
                                <div className="text-2xl font-bold pb-4">2. Book a Reservation</div>
                                <p className="text-m w-3/4">
                                    Your table, your choice, effortlessly reserved through our platform! From cozy cafes to chic bars, exclusive clubs to top-rated restaurants, unlock seamless booking and indulge in extraordinary experiences. Your next adventure starts with a simple click. Reserve now and make every moment memorable.
                                </p>
                            </CardContent>
                        </Card>
                        <Card x-chunk="dashboard-01-chunk-1" className="border-0 shadow-none bg-transparent text-white">
                            <CardHeader className="flex flex-row items-center justify-center space-y-0 pb-8">
                                <CardTitle className="text-sm font-medium">
                                    <FastForward color="#ffffff" className="h-12 w-12 text-muted-foreground" />
                                </CardTitle>
                            </CardHeader>
                            <CardContent className="flex flex-col items-center justify-center text-center">
                                <div className="text-2xl font-bold pb-4">3. Await Booking Confirmation</div>
                                <p className="text-m w-3/4">
                                    Excitement awaits! Your booking request has been sent. Stay tuned for your confirmation to unlock the door to your next adventure. Get ready to indulge, explore, and create unforgettable memories. The journey begins soon!
                                </p>
                            </CardContent>
                        </Card>
                    </div>
                </div>
                <div className="w-full rounded-lg text-white flex flex-col items-center justify-center">
                    <a href="/locations" className="group border-white border-2 inline-flex h-10 w-max items-center justify-center rounded-md bg-transparent px-4 py-2 text-sm text-white font-medium transition-colors hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground focus:outline-none disabled:pointer-events-none disabled:opacity-50 data-[active]:bg-accent/50 data-[state=open]:bg-accent/50">
                        Get Started
                    </a>
                </div>
            </div>
        </div>
    )
}

export default HowItWorks