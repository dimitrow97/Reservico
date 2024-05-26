import { Loader2 } from 'lucide-react'
import { cn } from '@/lib/utils'

const Loader = () => {
   return (
        <div className="flex-col items-center justify-center">
            <Loader2 className={cn('h-4 w-4 animate-spin mr-2')} /> 
            <span>Loading...</span>
        </div>
   )
}

export default Loader