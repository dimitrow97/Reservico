import { apiSlice } from "../../app/api/api-slice";

export const authApiSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        login: builder.mutation({
            query: credentials => ({
                url: '/Authorize/Login',
                method: 'POST',
                body: { ...credentials }
            })
        }),
        token: builder.mutation({
            query: params => ({
                url: '/Token',
                method: 'GET',
                params: { ...params }
            })
        }),
    })
})

export const {
    useLoginMutation,
    useTokenMutation
} = authApiSlice