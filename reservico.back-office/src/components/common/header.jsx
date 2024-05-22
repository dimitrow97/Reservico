import ProfileDropdown from './profile-dropdown'

const Header = () => {
    return (
        <div className="w-full grid grid-cols-2 gap-4 border-b p-4">
            <div className="flex items-center justify-center">
            </div>
            <div className="flex items-center justify-end">
                <ProfileDropdown />
            </div>
        </div>
    )
}

export default Header