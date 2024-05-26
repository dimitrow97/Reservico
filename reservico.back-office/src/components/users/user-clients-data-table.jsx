import * as React from "react"
import {
    flexRender,
    getCoreRowModel,
    getFilteredRowModel,
    getPaginationRowModel,
    getSortedRowModel,
    useReactTable,
} from "@tanstack/react-table"
import { ArrowUpDown, ChevronDown, MoreHorizontal } from "lucide-react"

import { Button } from "@/components/ui/button"
import { Checkbox } from "@/components/ui/checkbox"
import {
    DropdownMenu,
    DropdownMenuCheckboxItem,
    DropdownMenuContent,
    DropdownMenuItem,
    DropdownMenuLabel,
    DropdownMenuSeparator,
    DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import { Input } from "@/components/ui/input"
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table"
import {Link, useNavigate} from 'react-router-dom';
import { useToast } from "@/components/ui/use-toast"
import { useRemoveUserFromClientMutation, useReactivateUserClientMutation } from "../../features/users/users-api-slice"
import { useDispatch } from "react-redux"
import { apiSlice } from "../../app/api/api-slice"
import UserAddClientDrawer from "./user-add-client-drawer"

export const columns = [
    {
        id: "select",
        header: ({ table }) => (
            <Checkbox
                checked={
                    table.getIsAllPageRowsSelected() ||
                    (table.getIsSomePageRowsSelected() && "indeterminate")
                }
                onCheckedChange={(value) => table.toggleAllPageRowsSelected(!!value)}
                aria-label="Select all"
            />
        ),
        cell: ({ row }) => (
            <Checkbox
                checked={row.getIsSelected()}
                onCheckedChange={(value) => row.toggleSelected(!!value)}
                aria-label="Select row"
            />
        ),
        enableSorting: false,
        enableHiding: false,
    },
    {
        accessorKey: "clientId",
        header: ({ column }) => {
            return (
                <Button
                    variant="ghost"
                    onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
                >
                    Id
                    <ArrowUpDown className="ml-2 h-4 w-4" />
                </Button>
            )
        },
        cell: ({ row }) => <div className="lowercase">{row.getValue("clientId")}</div>,
    },    
    {
        accessorKey: "clientName",
        header: ({ column }) => {
            return (
                <Button
                    variant="ghost"
                    onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
                >
                    Name
                    <ArrowUpDown className="ml-2 h-4 w-4" />
                </Button>
            )
        },
        cell: ({ row }) => <div>{row.getValue("clientName")}</div>,
    },    
    {
        accessorKey: "isDeleted",
        header: ({ column }) => {
            return (
                <Button
                    variant="ghost"
                    onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
                >
                    Is Deleted?
                    <ArrowUpDown className="ml-2 h-4 w-4" />
                </Button>
            )
        },
        cell: ({ row }) => <div>{row.original.isDeleted.toString()}</div>,
    },    
    {
        id: "actions",
        enableHiding: false,
        cell: ({ row }) => {
            const client = row.original          

            const [removeUser] = useRemoveUserFromClientMutation()
            const [reactivate] = useReactivateUserClientMutation()
            const { toast } = useToast()
            const dispatch = useDispatch()

            const removeClientFromUser = async ({ userId, clientId }) => {

                const request = {
                    userId: userId,
                    clientId: clientId
                }

                try {
                    const response = await removeUser(request).unwrap()
        
                    if (response.isSuccess) {
                        dispatch(apiSlice.util.invalidateTags(["client-users"]))
                        toast({
                            title: "Removed successfully!",
                            description: "Client was successfully removed from the User!",
                        })
                    }
                    else {
                        toast({
                            title: "Removing the Client was unsuccessfull!",
                            description: response.errorMessage,
                        })
                    }
                } catch (err) {
                    console.log(err);
                }
            }            

            const reactivateClient = async ({ userId, clientId }) => {

                const request = {
                    userId: userId,
                    clientId: clientId
                }

                try {
                    const response = await reactivate(request).unwrap()
        
                    if (response.isSuccess) {
                        dispatch(apiSlice.util.invalidateTags(["client-users"]))
                        toast({
                            title: "Reactivated successfully!",
                            description: "Client was successfully reactivated for the User!",
                        })
                    }
                    else {
                        toast({
                            title: "Reactivating the Client was unsuccessfull!",
                            description: response.errorMessage,
                        })
                    }
                } catch (err) {
                    console.log(err);
                }
            }            

            return (
                <DropdownMenu>
                    <DropdownMenuTrigger asChild>
                        <Button variant="ghost" className="h-8 w-8 p-0">
                            <span className="sr-only">Open menu</span>
                            <MoreHorizontal className="h-4 w-4" />
                        </Button>
                    </DropdownMenuTrigger>
                    <DropdownMenuContent align="end">
                        <DropdownMenuLabel>Actions</DropdownMenuLabel>
                        <DropdownMenuItem
                            onClick={() => navigator.clipboard.writeText(client.userId)}
                        >
                            Copy Client Id
                        </DropdownMenuItem>
                        <DropdownMenuSeparator />
                        <DropdownMenuItem>
                            <a onClick={() => {removeClientFromUser({ userId: client.userId, clientId: client.clientId })}}>Remove Client from User</a>
                        </DropdownMenuItem>
                        <DropdownMenuItem>
                            <a onClick={() => {reactivateClient({ userId: client.userId, clientId: client.clientId })}}>Reactivate Client</a>
                        </DropdownMenuItem>
                    </DropdownMenuContent>
                </DropdownMenu>
            )
        },
    },
]

const UserClientsDataTable = ({ data, userId }) => {
    const [sorting, setSorting] = React.useState([])
    const [columnFilters, setColumnFilters] = React.useState([])
    const [columnVisibility, setColumnVisibility] =
        React.useState({})
    const [rowSelection, setRowSelection] = React.useState({})
    
    const table = useReactTable({
        data,
        columns,
        onSortingChange: setSorting,
        onColumnFiltersChange: setColumnFilters,
        getCoreRowModel: getCoreRowModel(),
        getPaginationRowModel: getPaginationRowModel(),
        getSortedRowModel: getSortedRowModel(),
        getFilteredRowModel: getFilteredRowModel(),
        onColumnVisibilityChange: setColumnVisibility,
        onRowSelectionChange: setRowSelection,
        state: {
            sorting,
            columnFilters,
            columnVisibility,
            rowSelection,
        },
    })
    
    return (
        <div className="w-full">
            <div className="flex items-center py-4">
                <Input
                    placeholder="Filter name..."
                    value={(table.getColumn("clientName")?.getFilterValue()) ?? ""}
                    onChange={(event) =>
                        table.getColumn("clientName")?.setFilterValue(event.target.value)
                    }
                    className="max-w-sm"
                />
                <UserAddClientDrawer userId={userId} />
                <DropdownMenu>
                    <DropdownMenuTrigger asChild>                        
                        <Button variant="outline" className="ml-2">
                            Columns <ChevronDown className="ml-2 h-4 w-4" />
                        </Button>
                    </DropdownMenuTrigger>
                    <DropdownMenuContent align="end">
                        {table
                            .getAllColumns()
                            .filter((column) => column.getCanHide())
                            .map((column) => {
                                return (
                                    <DropdownMenuCheckboxItem
                                        key={column.id}
                                        className="capitalize"
                                        checked={column.getIsVisible()}
                                        onCheckedChange={(value) =>
                                            column.toggleVisibility(!!value)
                                        }
                                    >
                                        {column.id}
                                    </DropdownMenuCheckboxItem>
                                )
                            })}
                    </DropdownMenuContent>
                </DropdownMenu>
            </div>
            <div className="rounded-md border">
                <Table>
                    <TableHeader>
                        {table.getHeaderGroups().map((headerGroup) => (
                            <TableRow key={headerGroup.id}>
                                {headerGroup.headers.map((header) => {
                                    return (
                                        <TableHead key={header.id}>
                                            {header.isPlaceholder
                                                ? null
                                                : flexRender(
                                                    header.column.columnDef.header,
                                                    header.getContext()
                                                )}
                                        </TableHead>
                                    )
                                })}
                            </TableRow>
                        ))}
                    </TableHeader>
                    <TableBody>
                        {table.getRowModel().rows?.length ? (
                            table.getRowModel().rows.map((row) => (
                                <TableRow
                                    key={row.id}
                                    data-state={row.getIsSelected() && "selected"}
                                >
                                    {row.getVisibleCells().map((cell) => (
                                        <TableCell key={cell.id}>
                                            {flexRender(
                                                cell.column.columnDef.cell,
                                                cell.getContext()
                                            )}
                                        </TableCell>
                                    ))}
                                </TableRow>
                            ))
                        ) : (
                            <TableRow>
                                <TableCell
                                    colSpan={columns.length}
                                    className="h-24 text-center"
                                >
                                    No results.
                                </TableCell>
                            </TableRow>
                        )}
                    </TableBody>
                </Table>
            </div>
            <div className="flex items-center justify-end space-x-2 py-4">
                <div className="flex-1 text-sm text-muted-foreground">
                    {table.getFilteredSelectedRowModel().rows.length} of{" "}
                    {table.getFilteredRowModel().rows.length} row(s) selected.
                </div>
                <div className="space-x-2">
                    <Button
                        variant="outline"
                        size="sm"
                        onClick={() => table.previousPage()}
                        disabled={!table.getCanPreviousPage()}
                    >
                        Previous
                    </Button>
                    <Button
                        variant="outline"
                        size="sm"
                        onClick={() => table.nextPage()}
                        disabled={!table.getCanNextPage()}
                    >
                        Next
                    </Button>
                </div>
            </div>
        </div>
    )
}

export default UserClientsDataTable