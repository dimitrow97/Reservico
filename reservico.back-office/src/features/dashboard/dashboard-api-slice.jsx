import { apiSlice } from "../../app/api/api-slice"

export const dashboardApiSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        getDashboard: builder.query({
            query: clientId => '/Dashboard',
            keepUnusedDataFor: 1,
            providesTags: ["dashboard"]
        })        
    })
})

export const {
    useGetDashboardQuery
} = dashboardApiSlice 