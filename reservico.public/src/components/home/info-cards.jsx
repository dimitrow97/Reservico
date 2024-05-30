import {
  FastForward,
  Sparkles,
  MapPin
} from "lucide-react"
import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"

const InfoCards = () => {
  return (
    <div className="flex flex-1 flex-col gap-4 p-4 md:gap-8 md:p-8">
      <div className="grid gap-4 md:grid-cols-3 md:gap-8 lg:grid-cols-3">
        <Card x-chunk="dashboard-01-chunk-0" className="border-0 shadow-none">
          <CardHeader className="flex flex-row items-center justify-center space-y-0 pb-8">
            <CardTitle className="text-sm font-medium">
              <Sparkles className="h-12 w-12 text-muted-foreground" />
            </CardTitle>
          </CardHeader>
          <CardContent className="flex flex-col items-center justify-center text-center">
            <div className="text-2xl font-bold pb-4">New Experiences</div>
            <p className="text-m text-muted-foreground w-2/3">
              Embrace the unknown! Dive into new adventures, explore uncharted territories, and cherish every moment of discovery. Life's greatest treasures are found outside your comfort zone. Take the leap, and let the magic of new experiences enrich your journey.
            </p>
          </CardContent>
        </Card>
        <Card x-chunk="dashboard-01-chunk-2" className="border-0 shadow-none">
          <CardHeader className="flex flex-row items-center justify-center space-y-0 pb-8">
            <CardTitle className="text-sm font-medium">
              <MapPin className="h-12 w-12 text-muted-foreground" />
            </CardTitle>
          </CardHeader>
          <CardContent className="flex flex-col items-center justify-center text-center">
            <div className="text-2xl font-bold pb-4">Numerous Locations</div>
            <p className="text-m text-muted-foreground w-2/3">
            Discover, indulge, savor! From cozy cafes to vibrant clubs, eclectic bars to exquisite restaurants, there's a world of flavors and experiences waiting for you. Explore the diverse tapestry of locations, each offering its own unique charm and delights. Cheers to endless adventures and unforgettable moments!
            </p>
          </CardContent>
        </Card>
        <Card x-chunk="dashboard-01-chunk-1" className="border-0 shadow-none">
          <CardHeader className="flex flex-row items-center justify-center space-y-0 pb-8">
            <CardTitle className="text-sm font-medium">
              <FastForward className="h-12 w-12 text-muted-foreground" />
            </CardTitle>
          </CardHeader>
          <CardContent className="flex flex-col items-center justify-center text-center">
            <div className="text-2xl font-bold pb-4">Express Booking</div>
            <p className="text-m text-muted-foreground w-2/3">
            Book now, experience soon! Secure your spot and unlock unforgettable memories. Your next adventure awaits with just a click. Seize the moment and create lasting stories to cherish.
            </p>
          </CardContent>
        </Card>        
      </div>
    </div>
  )
}

export default InfoCards