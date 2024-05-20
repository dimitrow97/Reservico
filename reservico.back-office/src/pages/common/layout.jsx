import { Outlet } from "react-router-dom"
import Sidebar from "../../components/common/side-bar"

const Layout = () => {
    return (
        <div className="flex items-start justify-between">
            <Sidebar />
            <div className="w-full h-full">
                <Outlet />
            </div>            
        </div>
    )
}

export default Layout