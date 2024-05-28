import { apiSlice } from "../../app/api/api-slice"

export const categoriesApiSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        getCategories: builder.query({
            query: () => '/Category',
            keepUnusedDataFor: 1,
            providesTags: ["category"]
        })        
    })
})

export const {
    useGetCategoriesQuery,   
} = categoriesApiSlice 