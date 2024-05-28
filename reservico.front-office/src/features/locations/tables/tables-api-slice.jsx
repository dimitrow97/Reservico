import { apiSlice } from "../../../app/api/api-slice"

export const tablesApiSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({        
        getTables: builder.query({
            query: locationId => '/Table/GetAll/' + locationId,
            keepUnusedDataFor: 1,
            providesTags: ["tables"]
        }), 
        getTable: builder.query({
            query: tableId => '/Table/' + tableId,
            keepUnusedDataFor: 1,
            providesTags: ["tables"]
        }), 
        addTable: builder.mutation({
            query: requestModel => ({
                url: '/Table',
                method: 'POST',
                body: { ...requestModel }
            }),
            invalidatesTags: ["tables"] 
        }),  
        deleteTable: builder.mutation({
            query: params => ({
                url: '/Table',
                method: 'DELETE',
                params: { ...params }
            }),
            invalidatesTags: ["tables"] 
        }),              
    })
})

export const {
    useGetTablesQuery,
    useGetTableQuery,
    useAddTableMutation,
    useDeleteTableMutation
} = tablesApiSlice 