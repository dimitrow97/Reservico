import { createSlice } from "@reduxjs/toolkit"

const authSlice = createSlice({
    name: 'auth',
    initialState: { user: null, token: null, refreshToken: null, roles: null },
    reducers: {
        setCredentials: (state, action) => {
            const { user, accessToken, refreshToken, roles } = action.payload
            state.user = user
            state.token = accessToken
            state.refreshToken = refreshToken
            state.roles = roles
        },
        logOut: (state, action) => {
            state.user = null
            state.token = null
            state.refreshToken = null
            state.roles = null
        }
    },
})

export const { setCredentials, logOut } = authSlice.actions

export default authSlice.reducer

export const selectCurrentUser = (state) => state.auth.user
export const selectCurrentToken = (state) => state.auth.token
export const selectCurrentRefreshToken = (state) => state.auth.refreshToken
export const selectCurrentRoles = (state) => state.auth.roles