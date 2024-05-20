import { apiSlice } from "../../app/api/api-slice"

export const usersApiSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        getUsers: builder.query({
            query: () => '/Admin/UserAdministration/GetAll',
            keepUnusedDataFor: 1,
        })
    })
})

export const {
    useGetUsersQuery
} = usersApiSlice 