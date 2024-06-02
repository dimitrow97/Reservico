import { createSlice } from "@reduxjs/toolkit"

const authSlice = createSlice({
    name: 'auth',
    initialState: { token: null, refreshToken: null },
    reducers: {
        setCredentials: (state, action) => {
            const { accessToken, refreshToken } = action.payload            
            state.token = accessToken
            state.refreshToken = refreshToken
        },
        setCredetialsAfterRefresh: (state, action) => {
            const { accessToken, refreshToken } = action.payload            
            state.token = accessToken
            state.refreshToken = refreshToken
        },
        logOut: (state, action) => {            
            state.token = null
            state.refreshToken = null            
        }
    },
})

export const { setCredentials, setCredetialsAfterRefresh, logOut } = authSlice.actions

export default authSlice.reducer

export const selectCurrentToken = (state) => state.auth.token
export const selectCurrentRefreshToken = (state) => state.auth.refreshToken