import { Routes, Route } from 'react-router-dom'
import Login from './pages/login'
import Home from './pages/home'
import Clients from './pages/clients'
import Layout from './pages/common/layout'
import ClientDetails from './pages/client-details'
import AccountProfile from './pages/account-profile'
import RequireAuth from './features/auth/require-auth'
import './App.css'

function App() {

  return (
    <Routes>
        {/* public routes */}
      <Route path="login" element={<Login />} />

        {/* protected routes */}
      <Route path="/" element={<Layout />}>
        <Route element={<RequireAuth />}>
          <Route path="home" element={<Home />} />
          <Route path="clients" element={<Clients />} />
          <Route path="client-details" element={<ClientDetails />} />
          <Route path="account-profile" element={<AccountProfile />} />
        </Route>
      </Route>
    </Routes>
      
  )
}

export default App
