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
        console.log('sending refresh token')
        // send refresh token to get new access token 
        const grantType = "refresh_token";        
        const refreshToken = api.getState().auth.refreshToken;
        const refreshResult = await baseQuery( {
                url: '/Token/Refresh',
                method: 'POST',
                body: { granttype: grantType, refreshToken: refreshToken } 
            }, api, extraOptions)
        console.log(refreshResult)
        if (refreshResult?.data) {
            const user = api.getState().auth.user
            // store the new token 
            api.dispatch(setCredetialsAfterRefresh({ ...refreshResult.data, user }))
            // retry the original query with new access token 
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
        'client-details',
        'client-users'
    ],
    endpoints: builder => ({})
})