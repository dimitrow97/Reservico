import {
    Send
} from "lucide-react"
import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
} from "@/components/ui/card"

const GotQuestions = () => {
    return (
        <div className="rounded-lg p-8 text-white flex flex-col items-center justify-center">
            <Card x-chunk="dashboard-01-chunk-0" className="border-0 shadow-none">
                <CardHeader className="flex flex-row items-center justify-center space-y-0 pb-8">
                    <CardTitle className="text-sm font-medium">
                        <Send className="h-12 w-12 text-muted-foreground" />
                    </CardTitle>
                </CardHeader>
                <CardContent className="flex flex-col items-center justify-center text-center">
                    <div className="text-3xl p-4 text-slate-600 font-bold border-4 border-slate-600 border-spacing-4">Got any Questions? Need Help?</div>
                    <p className="text-lg text-slate-500 pt-4 w-2/3">
                       We are here to help. Feel free to contact us at: reservico.app@gmail.com
                    </p>
                </CardContent>
            </Card>
        </div>
    )
}

export default GotQuestions