import { apiSlice } from "../../app/api/api-slice"

export const reservationsApiSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        getReservations: builder.query({
            query: clientId => '/Reservation/GetAll?clientId=' + clientId,
            keepUnusedDataFor: 1,
            providesTags: ["reservations"]
        }),
        getReservationDetails: builder.query({
            query: reservationId => '/Reservation/' + reservationId,
            keepUnusedDataFor: 1,            
            providesTags: ["reservations"]
        }),    
        confirmReservation: builder.mutation({
            query: params => ({
                url: '/Reservation/Confirm',
                method: 'POST',
                params: { ...params }
            }),
            invalidatesTags: ["reservations", "dashboard"] 
        }),       
    })
})

export const {
    useGetReservationsQuery,
    useGetReservationDetailsQuery,
    useConfirmReservationMutation    
} = reservationsApiSlice 