import { Routes, Route } from 'react-router-dom'
import Login from './pages/login'
import Layout from './pages/common/layout'
import Home from './pages/home'
import AccountProfile from './pages/account-profile'
import Locations from './pages/locations'
import LocationDetails from './pages/location-details'
import TableDetails from './pages/table-details'
import Reservations from './pages/reservations'
import ReservationDetails from './pages/reservation-details'
import RequireAuth from './features/auth/require-auth'
import './App.css'
import NotFound from './pages/not-found'

function App() {

  return (
    <Routes>
      {/* public routes */}
      <Route path="login" element={<Login />} />

      {/* protected routes */}
      <Route element={<RequireAuth allowedRoles={ ["Read-Write_User"] } />}>
        <Route element={<Layout />}>
          <Route path="/" element={<Home />} />
          <Route path="account-profile" element={<AccountProfile />} />
          <Route path="locations" element={<Locations />} />
          <Route path="location-details" element={<LocationDetails />} />
          <Route path="table-details" element={<TableDetails />} />
          <Route path="reservations" element={<Reservations />} />
          <Route path="reservation-details" element={<ReservationDetails />} />
          <Route path="*" element={<NotFound />} />
        </Route>
      </Route>
    </Routes>

  )
}

export default App
