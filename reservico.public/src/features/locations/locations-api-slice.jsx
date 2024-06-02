import { apiSlice } from "../../app/api/api-slice"

export const locationsApiSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({        
        getLocations: builder.query({
            query: params => '/Public/Location/GetAll?filter=' + params.filter + '&skip=' + params.skip + '&take=' + params.take,
            keepUnusedDataFor: 1,
            providesTags: ["locations"]
        }),
        getLocation: builder.query({
            query: locationId => '/Public/Location/' + locationId,
            keepUnusedDataFor: 1,
            providesTags: ["locations"]
        })        
    })
})

export const {
    useGetLocationsQuery,
    useGetLocationQuery
} = locationsApiSlice 