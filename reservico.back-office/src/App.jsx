import { useState } from 'react'
import { Routes, Route } from 'react-router-dom'
import Login from './pages/login'
import Home from './pages/home'
import Layout from './pages/common/layout'
import RequireAuth from './features/auth/require-auth'
import './App.css'

function App() {
  const [count, setCount] = useState(0)

  return (
    <Routes>
        {/* public routes */}
      <Route path="login" element={<Login />} />

        {/* protected routes */}
      <Route path="/" element={<Layout />}>
        <Route element={<RequireAuth />}>
          <Route path="home" element={<Home />} />
        </Route>
      </Route>
    </Routes>
      
  )
}

export default App
