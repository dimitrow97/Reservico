import { apiSlice } from "../../app/api/apiSlice"

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