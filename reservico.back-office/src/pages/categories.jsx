import CategoriesDataTable from "../components/categories/categories-data-table"
import Loader from "../components/common/loader"
import { useGetCategoriesQuery } from "../features/categories/categories-api-slice"

import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
  } from "@/components/ui/card"

const Categories = () => {
    const {
        data: categories,
        isLoading,
        isSuccess,
        isError,
        error
    } = useGetCategoriesQuery()

    let content;
    if (isLoading) {
        content = <Loader />;
    } else if (isSuccess) {
        content = (
            <Card className="w-full h-full">
                <CardHeader>
                    <CardTitle>Categories</CardTitle>
                </CardHeader>
                <CardContent className="grid gap-4">
                    <CategoriesDataTable data={categories.data} />
                </CardContent>
            </Card>
        )
    } else if (isError) {
        content = <p>{JSON.stringify(error)}</p>;
    }

    return content
}

export default Categories