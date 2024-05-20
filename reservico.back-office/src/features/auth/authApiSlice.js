import { apiSlice } from "../../app/api/apiSlice";

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
                method: 'Get',
                params: { ...params }
            })
        }),
    })
})

export const {
    useLoginMutation,
    useTokenMutation
} = authApiSlice