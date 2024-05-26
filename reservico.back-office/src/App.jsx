import { Routes, Route } from 'react-router-dom'
import Login from './pages/login'
import Layout from './pages/common/layout'
import Home from './pages/home'
import Clients from './pages/clients'
import ClientDetails from './pages/client-details'
import AccountProfile from './pages/account-profile'
import Users from './pages/users'
import UserDetails from './pages/user-details'
import Categories from './pages/categories'
import RequireAuth from './features/auth/require-auth'
import './App.css'

function App() {

  return (
    <Routes>
      {/* public routes */}
      <Route path="login" element={<Login />} />

      {/* protected routes */}
      <Route element={<RequireAuth />}>
        <Route path="/" element={<Layout />}>
          <Route path="home" element={<Home />} />
          <Route path="clients" element={<Clients />} />
          <Route path="client-details" element={<ClientDetails />} />
          <Route path="account-profile" element={<AccountProfile />} />
          <Route path="users" element={<Users />} />
          <Route path="user-details" element={<UserDetails />} />
          <Route path="categories" element={<Categories />} />
        </Route>
      </Route>
    </Routes>

  )
}

export default App
