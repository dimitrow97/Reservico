import Navigation from "./navigation"

const HeaderHome = () => {
    return (
        <div className="w-full absolute grid grid-cols-2 gap-4 p-4 bg-transparent">
            <div className="flex items-center justify-start">
                <a href="/" className="text-white">                    
                    <div className="flex flex-row gap-1 pl-2">
                        <img src="/logo-home.svg" className="fill-white" />
                        <span className="text-xl">Reservico</span>
                    </div>
                </a>
            </div>
            <div className="flex items-center justify-end">
                <Navigation />
            </div>
        </div>
    )
}

export default HeaderHome