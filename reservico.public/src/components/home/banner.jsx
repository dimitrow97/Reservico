import Image from "../ui/image"
import {
    ArrowDownToLine
} from "lucide-react"
import { Separator } from "@/components/ui/separator"

const Banner = () => {
    return (
        <div className="h-[400px] xl:h-[600px] bg-[url('/banner.jpg')] bg-center bg-cover overflow-hidden">
            <div className="absolute xl:w-1/2 xl:h-1/2 top-24 md:top-48 xl:top-1/4 left-4 md:left-40 xl:left-1/4 rounded-lg p-4 text-white flex flex-col items-center justify-center">
                <span className="text-lg text-center md:text-4xl font-bold pb-2 drop-shadow-[0_1.2px_1.2px_rgba(0,0,0,0.8)]">
                    UPGRADE YOUR BOOKING EXPERIENCE
                </span>
                <Separator className="w-1/6 h-1" />   
                <span className="text-center text-sm md:text-lg xl:text-xl mt-auto pb-2 drop-shadow-[0_1.2px_1.2px_rgba(0,0,0,0.8)]">
                    Discover all the places you can book
                </span>
                <ArrowDownToLine className="mt-2 md:mt-8 xl:mt-8 h-2 md:h-8 xl:h-8 w-2 md:w-8 xl:w-8 drop-shadow-[0_1.2px_1.2px_rgba(0,0,0,0.8)]" />             
            </div>
        </div>
    )
}

export default Banner