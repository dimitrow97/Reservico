import { Loader2 } from 'lucide-react'
import { cn } from '@/lib/utils'

const Loader = () => {
    return (
        <div className="flex flex-1 w-full h-full flex-col justify-center items-center gap-4 p-4 md:gap-8 md:p-8">
            <div className="grid gap-4 md:gap-8">
                <Loader2 className={cn('h-8 w-8 animate-spin ml-auto mr-2')} />
                <span className="text-lg">Loading...</span>
            </div>
        </div>
    )
}

export default Loader