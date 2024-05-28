import { createSlice } from "@reduxjs/toolkit"

const authSlice = createSlice({
    name: 'auth',
    initialState: { userData: null, token: null, refreshToken: null, email: null, clientId: null },
    reducers: {
        setCredentials: (state, action) => {
            const { userData, accessToken, refreshToken, email } = action.payload
            state.userData = userData
            state.token = accessToken
            state.refreshToken = refreshToken
            state.email = email
            state.clientId = userData.clientId
        },
        setCredetialsAfterRefresh: (state, action) => {
            const { accessToken, refreshToken } = action.payload            
            state.token = accessToken
            state.refreshToken = refreshToken
        },
        setUserSelectedClient: (state, action) => {
            const { clientId } = action.payload           
            state.clientId = clientId
        },
        logOut: (state, action) => {
            state.userData = null
            state.token = null
            state.refreshToken = null
            state.email = null
            state.clientId = null
        }
    },
})

export const { setCredentials, setCredetialsAfterRefresh, setUserSelectedClient, logOut } = authSlice.actions

export default authSlice.reducer

export const selectCurrentUserData = (state) => state.auth.userData
export const selectCurrentToken = (state) => state.auth.token
export const selectCurrentRefreshToken = (state) => state.auth.refreshToken
export const selectCurrentEmail = (state) => state.auth.email
export const selectCurrentClient = (state) => state.auth.clientId