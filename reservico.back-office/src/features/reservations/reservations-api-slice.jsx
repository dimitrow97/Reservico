import { apiSlice } from "../../app/api/api-slice"

export const reservationsApiSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        getReservations: builder.query({
            query: () => '/Reservation/GetAll',
            keepUnusedDataFor: 1,
            providesTags: ["reservations"]
        }),
        getReservationDetails: builder.query({
            query: reservationId => '/Reservation/' + reservationId,
            keepUnusedDataFor: 1,            
            providesTags: ["reservations"]
        }),        
    })
})

export const {
    useGetReservationsQuery,
    useGetReservationDetailsQuery    
} = reservationsApiSlice 