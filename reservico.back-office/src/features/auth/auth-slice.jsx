import { createSlice } from "@reduxjs/toolkit"

const authSlice = createSlice({
    name: 'auth',
    initialState: { userData: null, token: null, refreshToken: null, email: null },
    reducers: {
        setCredentials: (state, action) => {
            const { userData, accessToken, refreshToken, email } = action.payload
            state.userData = userData
            state.token = accessToken
            state.refreshToken = refreshToken
            state.email = email
        },
        setCredetialsAfterRefresh: (state, action) => {
            const { accessToken, refreshToken } = action.payload            
            state.token = accessToken
            state.refreshToken = refreshToken
        },
        logOut: (state, action) => {
            state.userData = null
            state.token = null
            state.refreshToken = null
            state.email = null
        }
    },
})

export const { setCredentials, setCredetialsAfterRefresh, logOut } = authSlice.actions

export default authSlice.reducer

export const selectCurrentUserData = (state) => state.auth.userData
export const selectCurrentToken = (state) => state.auth.token
export const selectCurrentRefreshToken = (state) => state.auth.refreshToken
export const selectCurrentEmail = (state) => state.auth.email