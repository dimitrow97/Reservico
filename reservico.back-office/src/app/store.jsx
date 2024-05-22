import { combineReducers } from 'redux'
import { configureStore } from "@reduxjs/toolkit"
import { apiSlice } from "./api/api-slice"
import { persistStore, persistReducer } from 'redux-persist'
import authReducer from '../features/auth/auth-slice'
import storage from 'redux-persist/lib/storage' // defaults to localStorage for web

const persistConfig = {
    key: 'root',
    storage,
  }

const rootReducer = combineReducers({ 
    [apiSlice.reducerPath]: apiSlice.reducer,
    auth: authReducer
})
  
const persistedReducer = persistReducer(persistConfig, rootReducer)
  
export const store = configureStore({
    reducer: persistedReducer,
    middleware: getDefaultMiddleware =>
        getDefaultMiddleware({ serializableCheck: false, }).concat(apiSlice.middleware),
    devTools: true
})

export const persistor = persistStore(store)