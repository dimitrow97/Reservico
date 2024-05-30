import Image from "../ui/image"
import {
    ArrowDownToLine
} from "lucide-react"
import { Separator } from "@/components/ui/separator"

const Banner = () => {
    return (
        <div className="h-[600px] overflow-hidden">
            <div className="w-1/2 h-1/2 absolute top-1/4 left-1/4 rounded-lg p-4 text-white flex flex-col items-center justify-center">
                <span className="text-4xl font-bold pb-2 drop-shadow-[0_1.2px_1.2px_rgba(0,0,0,0.8)]">
                    UPGRADE YOUR BOOKING EXPERIENCE
                </span>
                <Separator className="w-1/6 h-1" />   
                <span className="text-xl mt-auto pb-2 drop-shadow-[0_1.2px_1.2px_rgba(0,0,0,0.8)]">
                    Discover all the places you can book
                </span>
                <ArrowDownToLine className="mt-8 h-8 w-8 drop-shadow-[0_1.2px_1.2px_rgba(0,0,0,0.8)]" />             
            </div>
            <Image src="/banner.jpg" alt="Image" className="object-contain" />
        </div>
    )
}

export default Banner