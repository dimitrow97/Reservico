import './App.css'
import { Routes, Route } from 'react-router-dom'
import Layout from './pages/common/layout'
import LayoutHome from './pages/common/layout-home'
import Home from './pages/home'
import Locations from './pages/locations'
import LocationDetails from './pages/location-details'
import ReservationDetails from './pages/reservation-details'
import ReservationCancel from './pages/reservation-cancel'
import NotFound from './pages/not-found'

function App() {

  return (
    <Routes>
      {/* public routes */}
        <Route element={<LayoutHome />}>
          <Route path="/" element={<Home />} />
        </Route>
        <Route element={<Layout />}>
          <Route path="/locations" element={<Locations />} />
          <Route path="/location-details/:id" element={<LocationDetails />} />
          <Route path="/reservation-details/:id" element={<ReservationDetails />} />
          <Route path="/reservation/cancel/:id" element={<ReservationCancel />} />
          <Route path="*" element={<NotFound />} />
        </Route>
        
    </Routes>
  )
}

export default App
