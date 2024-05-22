import { Outlet } from "react-router-dom"
import Header from "../../components/common/header"
import Sidebar from "../../components/common/side-bar"
import { Toaster } from "@/components/ui/toaster"

const Layout = () => {
    return (
        <div className="flex items-start justify-between">
            <Sidebar />
            <main className="grid w-full h-full pl-[300px]">
                <Header />
                <div className="p-8">
                    <Outlet />
                </div>
            </main>
            <Toaster />           
        </div>
    )
}

export default Layout