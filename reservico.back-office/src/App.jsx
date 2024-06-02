import { Routes, Route } from 'react-router-dom'
import Login from './pages/login'
import Layout from './pages/common/layout'
import Home from './pages/home'
import Clients from './pages/clients'
import ClientDetails from './pages/client-details'
import AccountProfile from './pages/account-profile'
import Users from './pages/users'
import UserDetails from './pages/user-details'
import Reservations from './pages/reservations'
import ReservationDetails from './pages/reservation-details'
import Categories from './pages/categories'
import RequireAuth from './features/auth/require-auth'
import './App.css'
import NotFound from './pages/not-found'

function App() {

  return (
    <Routes>
      {/* public routes */}
      <Route path="login" element={<Login />} />

      {/* protected routes */}
      <Route element={<RequireAuth allowedRoles={ ["Admin"] }/>}>
        <Route element={<Layout />}>
          <Route path="/" element={<Home />} />
          <Route path="clients" element={<Clients />} />
          <Route path="client-details" element={<ClientDetails />} />
          <Route path="account-profile" element={<AccountProfile />} />
          <Route path="users" element={<Users />} />
          <Route path="user-details" element={<UserDetails />} />
          <Route path="reservations" element={<Reservations />} />
          <Route path="reservation-details" element={<ReservationDetails />} />
          <Route path="categories" element={<Categories />} />
          <Route path="*" element={<NotFound />} />
        </Route>
      </Route>
    </Routes>

  )
}

export default App
