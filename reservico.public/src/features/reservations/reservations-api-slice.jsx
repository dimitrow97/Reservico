import { apiSlice } from "../../app/api/api-slice"

export const reservationsApiSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        getReservationDetails: builder.query({
            query: reservationId => '/Reservation/' + reservationId,
            keepUnusedDataFor: 1,            
            providesTags: ["reservations"]
        }),    
        makeReservation: builder.mutation({
            query: body => ({
                url: '/Public/Reservation',
                method: 'POST',
                body: { ...body }
            }),
            invalidatesTags: ["reservations"] 
        }),       
        cancelReservation: builder.mutation({
            query: params => ({
                url: '/Public/Reservation',
                method: 'PUT',
                params: { ...params }
            }),
            invalidatesTags: ["reservations"] 
        }),       
        cancellationReservation: builder.mutation({
            query: body => ({
                url: '/Public/Reservation/Cancellation',
                method: 'POST',
                params: { ...body }
            }),
            invalidatesTags: ["reservations"] 
        }),       
    })
})

export const {
    useGetReservationDetailsQuery,
    useMakeReservationMutation,
    useCancelReservationMutation,
    useCancellationReservationMutation 
} = reservationsApiSlice 