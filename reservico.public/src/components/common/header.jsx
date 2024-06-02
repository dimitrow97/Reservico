import Navigation from "./navigation"

const Header = () => {
    return (
        <div className="w-full grid grid-cols-2 gap-4 p-4 bg-[#28282a]">
            <div className="flex items-center justify-start">
                <a href="/" className="text-white">                    
                    <div className="flex flex-row gap-1 pl-2">
                        <img src="/logo.svg" className="fill-white" />
                        <span className="text-2xl">Reservico</span>
                    </div>
                </a>
            </div>
            <div className="flex items-center justify-end">
                <Navigation />
            </div>
        </div>
    )
}

export default Header