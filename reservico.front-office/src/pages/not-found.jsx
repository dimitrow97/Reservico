import {
    Send
} from "lucide-react"
import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
} from "@/components/ui/card"

const NotFound = () => {
    return (
        <div className="rounded-lg p-8 text-white flex flex-col items-center justify-center">
            <Card x-chunk="dashboard-01-chunk-0" className="border-0 shadow-none">
                <CardHeader className="flex flex-row items-center justify-center space-y-0 pb-8">
                    <CardTitle className="text-5xl font-medium">
                        404
                    </CardTitle>
                </CardHeader>
                <CardContent className="flex flex-col items-center justify-center text-center">
                    <div className="text-3xl p-4 text-slate-600 font-bold border-4 border-slate-600 border-spacing-4">Ooops! The page you are looking for does not Exists</div>
                    <a href="/" className="text-lg text-slate-500 pt-4 w-2/3">
                       Click here to return to <strong>safety</strong>
                    </a>
                </CardContent>
            </Card>
        </div>
    )
}

export default NotFound