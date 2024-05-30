import { Routes, Route } from 'react-router-dom'
import Layout from './pages/common/layout'
import LayoutHome from './pages/common/layout-home'
import Home from './pages/home'
import './App.css'

function App() {

  return (
    <Routes>
      {/* public routes */}
        <Route element={<LayoutHome />}>
          <Route path="/" element={<Home />} />
        </Route>
    </Routes>
  )
}

export default App
