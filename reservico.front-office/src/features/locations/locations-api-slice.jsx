import { apiSlice } from "../../app/api/api-slice"

export const locationsApiSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({        
        getLocations: builder.query({
            query: () => '/Location',
            keepUnusedDataFor: 1,
            providesTags: ["locations"]
        }),   
        getLocationDetails: builder.query({
            query: locationId => '/Location/' + locationId,
            keepUnusedDataFor: 1,
            providesTags: ["locations"]
        }),   
        addLocation: builder.mutation({
            query: requestModel => ({
                url: '/Location',
                method: 'POST',
                body: { ...requestModel }
            }),
            invalidatesTags: ["locations", "dashboard"] 
        }),  
        deleteLocation: builder.mutation({
            query: params => ({
                url: '/Location',
                method: 'DELETE',
                params: { ...params }
            }),
            invalidatesTags: ["locations", "dashboard"] 
        }),  
        updateLocation: builder.mutation({
            query: requestModel => ({
                url: '/Location',
                method: 'PUT',
                body: { ...requestModel }
            }),
            invalidatesTags: ["locations"] 
        }),               
    })
})

export const {
    useGetLocationsQuery,
    useGetLocationDetailsQuery,
    useAddLocationMutation,
    useDeleteLocationMutation,
    useUpdateLocationMutation
} = locationsApiSlice 