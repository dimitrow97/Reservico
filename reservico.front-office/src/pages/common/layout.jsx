import { Outlet } from "react-router-dom"
import Header from "../../components/common/header"
import Sidebar from "../../components/common/side-bar"
import { Toaster } from "@/components/ui/toaster"

const Layout = () => {
    return (
        <div className="flex items-start justify-between">
            <div className="hidden md:flex min-w-[300px] border-r min-h-screen">
                <Sidebar />
            </div>
            <main className="grid w-full h-full">
                <Header />
                <div className="p-8">
                    <Outlet />
                </div>
            </main>
            <Toaster duration={10000} />
        </div>
    )
}

export default Layout