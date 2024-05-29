import { apiSlice } from "../../../app/api/api-slice"

export const imagesApiSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({        
        getLocationImages: builder.query({
            query: locationId => '/LocationImages/' + locationId,
            keepUnusedDataFor: 1,
            providesTags: ["images"]
        }), 
        uploadImage: builder.mutation({
            query: requestModel => ({
                url: '/LocationImages',
                method: 'POST',
                body: requestModel
            }),
            invalidatesTags: ["images"] 
        }),    
        deleteLocationImage: builder.mutation({
            query: params => ({
                url: '/LocationImages',
                method: 'DELETE',
                params: { ...params }
            }),
            invalidatesTags: ["images"] 
        }),            
    })
})

export const {
    useGetLocationImagesQuery,
    useUploadImageMutation,
    useDeleteLocationImageMutation
} = imagesApiSlice 