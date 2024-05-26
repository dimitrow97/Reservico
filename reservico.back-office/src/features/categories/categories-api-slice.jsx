import { apiSlice } from "../../app/api/api-slice"

export const categoriesApiSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        getCategories: builder.query({
            query: () => '/Admin/CategoryAdministration',
            keepUnusedDataFor: 1,
            providesTags: ["category"]
        }),      
        deleteCategory: builder.mutation({
            query: params => ({
                url: '/Admin/CategoryAdministration',
                method: 'DELETE',
                params: { ...params }
            }),
            invalidatesTags: ["category"] 
        }),
        addCategory: builder.mutation({
            query: body => ({
                url: '/Admin/CategoryAdministration',
                method: 'POST',
                body: { ...body }
            }),
            invalidatesTags: ["category"] 
        }),
    })
})

export const {
    useGetCategoriesQuery,   
    useAddCategoryMutation,
    useDeleteCategoryMutation,
} = categoriesApiSlice 