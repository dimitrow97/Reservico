import { apiSlice } from "../../app/api/api-slice"

export const clientsApiSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        getClients: builder.query({
            query: () => '/Admin/ClientAdministration',
            keepUnusedDataFor: 1,
        }),
        getClientDetails: builder.query({
            query: clientId => '/Admin/ClientAdministration/' + clientId,
            keepUnusedDataFor: 1,
        }),
        updateClient: builder.mutation({
            query: client => ({
                url: '/Admin/ClientAdministration',
                method: 'PUT',
                body: { ...client }
            })
        }),
        deleteClient: builder.mutation({
            query: params => ({
                url: '/Admin/ClientAdministration',
                method: 'DELETE',
                params: { ...params }
            })
        }),
        addClient: builder.mutation({
            query: client => ({
                url: '/Admin/ClientAdministration',
                method: 'POST',
                body: { ...client }
            })
        }),
    })
})

export const {
    useGetClientsQuery,
    useGetClientDetailsQuery,
    useUpdateClientMutation,
    useDeleteClientMutation,
    useAddClientMutation
} = clientsApiSlice 