import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'
import { setCredentials, setCredetialsAfterRefresh, logOut } from '../../features/auth/auth-slice'

const baseQuery = fetchBaseQuery({
    baseUrl: 'https://localhost:7161',
    prepareHeaders: async (headers, { getState }) => {
        const token = getState().auth.token

        if (token) {
            headers.set("authorization", `Bearer ${token}`)
        }

        return headers
    }
})

const getToken = async (api, extraOptions) => {
    const grantType = "client_credentials";
    const clientId = "1CE67DD0AE1CFC2FBB4883EFC4B5DFBAF58A939ACD93DC9A81A2DFE5747397BCE3260ADC70BF9EBE946C4B2C1460C67F";
    const clientSecret = "d41d8cd98f00b204e9800998ecf8427e";

    const tokenResult = await baseQuery({
        url: '/Token',
        method: 'POST',
        body: { granttype: grantType, clientId: clientId, clientSecret: clientSecret }
    }, api, extraOptions)

    if (tokenResult?.data) {
        api.dispatch(setCredentials({ ...tokenResult.data }))
    }
}

const baseQueryWithReauth = async (args, api, extraOptions) => {
    const token = api.getState().auth.token

    if (!token) {
        await getToken(api, extraOptions)
    }

    let result = await baseQuery(args, api, extraOptions)

    if (result?.error?.status === 401) {
        await getToken(api, extraOptions)

        result = await baseQuery(args, api, extraOptions)
    }

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