import { useSelector } from "react-redux"
import { selectCurrentUserData } from "../../features/auth/auth-slice"

const UserInfo = () => {
    const userData = useSelector(selectCurrentUserData)

    return (
        <div className="flex item-center justify-between gap-2 border rounded-[8px] p-2">
            <div className="avatar rounded-full min-h-10 min-w-10 bg-emerald-500 text-white font-[700] flex items-center justify-center">
                <p>{userData.firstName[0].toUpperCase()}{userData.lastName[0].toUpperCase()}</p>
            </div>
            <div className="grow">
                <p className="text-[16px] font-bold">{userData.firstName} {userData.lastName}</p>
                <p className="text-[12px] text-neutral-500">{userData.email}</p>
            </div>
        </div>
    )
}

export default UserInfo