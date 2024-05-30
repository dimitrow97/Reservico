import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'
import { setCredetialsAfterRefresh, logOut } from '../../features/auth/auth-slice'

const baseQuery = fetchBaseQuery({
    baseUrl: 'https://localhost:7161',
    prepareHeaders: (headers, { getState }) => {
        const token = getState().auth.token
        if (token) {
            headers.set("authorization", `Bearer ${token}`)
        }
        return headers
    }
})

const baseQueryWithReauth = async (args, api, extraOptions) => {
    let result = await baseQuery(args, api, extraOptions)

    if (result?.error?.status === 401) {
        const grantType = "refresh_token";        
        const refreshToken = api.getState().auth.refreshToken;

        const refreshResult = await baseQuery( {
                url: '/Token/Refresh',
                method: 'POST',
                body: { granttype: grantType, refreshToken: refreshToken } 
            }, api, extraOptions)
            
        if (refreshResult?.data) {
            const user = api.getState().auth.user
            
            api.dispatch(setCredetialsAfterRefresh({ ...refreshResult.data, user }))
            
            result = await baseQuery(args, api, extraOptions)
        } else {
            api.dispatch(logOut())
        }
    }

    return result
}

export const apiSlice = createApi({        
    baseQuery: baseQueryWithReauth,
    tagTypes: [
        "client", 
        "client-users", 
        "category", 
        "users",
        "locations",
        "tables",
        "reservations",
        "images",
        "dashboard"
    ],
    endpoints: builder => ({})
})