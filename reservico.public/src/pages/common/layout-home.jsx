import { Outlet } from "react-router-dom"
import HeaderHome from "../../components/common/header-home"
import { Toaster } from "@/components/ui/toaster"

const LayoutHome = () => {
    return (
        <div className="flex items-start justify-between">
            <main className="grid w-full h-full">                
                <div>
                    <HeaderHome />
                    <Outlet />
                </div>
            </main>
            <Toaster duration={10000} />
        </div>
    )
}

export default LayoutHome