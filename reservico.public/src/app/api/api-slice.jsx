import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'

const baseQuery = fetchBaseQuery({
    baseUrl: 'https://localhost:7161'
})

const baseQueryWithReauth = async (args, api, extraOptions) => {
    let result = await baseQuery(args, api, extraOptions)   

    return result
}

export const apiSlice = createApi({        
    baseQuery: baseQueryWithReauth,
    tagTypes: [         
        "category",         
        "locations",        
        "reservations"        
    ],
    endpoints: builder => ({})
})