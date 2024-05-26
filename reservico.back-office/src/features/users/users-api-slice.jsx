import { apiSlice } from "../../app/api/api-slice"

export const usersApiSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        getUser: builder.query({
            query: userId => '/Admin/UserAdministration/' + userId,
            keepUnusedDataFor: 1,
            providesTags: ["users"]
        }),
        getClientsForUser: builder.query({
            query: userId => '/Admin/UserClientAdministration/' + userId,
            keepUnusedDataFor: 1,
            providesTags: ["client-users"]
        }),
        getUsers: builder.query({
            query: () => '/Admin/UserAdministration/GetAll',
            keepUnusedDataFor: 1,
            providesTags: ["users"]
        }),
        getUsersForClient: builder.query({
            query: clientId => '/Users?clientId=' + clientId,
            keepUnusedDataFor: 1,
            providesTags: ["client-users"]
        }),
        changePassword: builder.mutation({
            query: requestModel => ({
                url: '/Users/ChangePassword',
                method: 'POST',
                body: { ...requestModel }
            })
        }),
        addUser: builder.mutation({
            query: requestModel => ({
                url: '/Admin/UserAdministration',
                method: 'POST',
                body: { ...requestModel }
            }),
            invalidatesTags: ["users"] 
        }),
        updateUser: builder.mutation({
            query: requestModel => ({
                url: '/Admin/UserAdministration',
                method: 'PUT',
                body: { ...requestModel }
            }),
            invalidatesTags: ["users"] 
        }),        
        removeUserFromClient: builder.mutation({
            query: requestModel => ({
                url: '/Admin/UserAdministration',
                method: 'DELETE',
                body: { ...requestModel }
            }),
            invalidatesTags: ["client-users"] 
        }),
        reactivateUserClient: builder.mutation({
            query: requestModel => ({
                url: '/Admin/UserClientAdministration/Reactivate',
                method: 'POST',
                body: { ...requestModel }
            }),
            invalidatesTags: ["client-users"] 
        }),
        addUserClient: builder.mutation({
            query: requestModel => ({
                url: '/Admin/UserClientAdministration',
                method: 'POST',
                body: { ...requestModel }
            }),
            invalidatesTags: ["client-users"] 
        }),
    })
})

export const {
    useGetUserQuery,
    useGetUsersQuery,
    useGetClientsForUserQuery,
    useGetUsersForClientQuery,
    useChangePasswordMutation,
    useAddUserMutation,
    useUpdateUserMutation,
    useRemoveUserFromClientMutation,
    useReactivateUserClientMutation,
    useAddUserClientMutation
} = usersApiSlice 