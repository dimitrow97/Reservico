import { apiSlice } from "../../app/api/api-slice"

export const usersApiSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({        
        getClientsForUser: builder.query({
            query: () => '/UserClient',
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
        updateUserSelectedClient: builder.mutation({
            query: requestModel => ({
                url: '/UserClient',
                method: 'PUT',
                body: { ...requestModel }
            }),
            invalidatesTags: ["client-users", "dashboard"] 
        }),                
    })
})

export const {
    useGetClientsForUserQuery,
    useChangePasswordMutation,
    useUpdateUserSelectedClientMutation
} = usersApiSlice 