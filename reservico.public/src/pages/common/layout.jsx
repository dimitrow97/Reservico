import { Outlet } from "react-router-dom"
import Header from "../../components/common/header"
import { Toaster } from "@/components/ui/toaster"

const Layout = () => {
    return (
        <div className="flex items-start justify-between">
            <main className="grid w-full h-full">
                <Header />
                <div>
                    <Outlet />
                </div>
            </main>
            <Toaster duration={10000} />
        </div>
    )
}

export default Layout