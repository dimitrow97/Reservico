import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import {
    Card,
    CardContent,
    CardDescription,
    CardHeader,
    CardTitle,
} from "@/components/ui/card"
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table"

const DashboardTable = ({ data }) => {
    return (
        <div className="grid gap-4 md:gap-8 lg:grid-cols-2 xl:grid-cols-3">
            <Card className="xl:col-span-3" x-chunk="dashboard-01-chunk-4">
                <CardHeader className="flex flex-row items-center">
                    <div className="grid gap-2">
                        <CardTitle>Reservations</CardTitle>
                        <CardDescription>
                            Recent reservations to your locations.
                        </CardDescription>
                    </div>
                    <Button asChild size="sm" className="ml-auto gap-1">
                    </Button>
                </CardHeader>
                <CardContent>
                    <Table>
                        <TableHeader>
                            <TableRow>
                                <TableHead>Customer</TableHead>
                                <TableHead>Location</TableHead>
                                <TableHead>Table</TableHead>
                                <TableHead>Guests Arriving At</TableHead>
                                <TableHead>Is Confirmed?</TableHead>
                            </TableRow>
                        </TableHeader>
                        <TableBody>
                            {data.map((reservation, key) => (
                                <TableRow key={key}>
                                    <TableCell>
                                        <div className="font-medium">{reservation.firstName + ' ' + reservation.lastName}</div>
                                        <div className="text-sm text-muted-foreground md:inline">
                                            {reservation.email}
                                        </div>
                                    </TableCell>
                                    <TableCell>
                                        {reservation.locationName}
                                    </TableCell>
                                    <TableCell>
                                        {reservation.tableName}
                                    </TableCell>
                                    <TableCell>
                                        {reservation.guestsArrivingAt}
                                    </TableCell>
                                    <TableCell>
                                        <Badge className="text-xs" variant="outline">
                                            {reservation.isConfirmed.toString()}
                                        </Badge>
                                    </TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </CardContent>
            </Card>
        </div>
    )
}

export default DashboardTable