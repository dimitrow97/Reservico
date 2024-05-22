import { apiSlice } from "../../app/api/api-slice"

export const usersApiSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        getUsers: builder.query({
            query: () => '/Admin/UserAdministration/GetAll',
            keepUnusedDataFor: 1,
        }),
        getUsersForClient: builder.query({
            query: clientId => '/Users?clientId=' + clientId,
            keepUnusedDataFor: 1,
            providesTags: ['client-users']
        }),
        changePassword: builder.mutation({
            query: requestModel => ({
                url: '/Users/ChangePassword',
                method: 'POST',
                body: { ...requestModel }
            })
        }),
        removeUserFromClient: builder.mutation({
            query: requestModel => ({
                url: '/Admin/UserAdministration',
                method: 'DELETE',
                body: { ...requestModel }
            }),
            invalidatesTags: ['client-users'] 
        }),
    })
})

export const {
    useGetUsersQuery,
    useGetUsersForClientQuery,
    useChangePasswordMutation,
    useRemoveUserFromClientMutation
} = usersApiSlice 