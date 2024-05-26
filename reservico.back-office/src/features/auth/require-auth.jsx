import { useLocation, Navigate, Outlet } from "react-router-dom"
import { useSelector } from "react-redux"
import { selectCurrentToken, selectCurrentUserData } from "./auth-slice"

const RequireAuth = () => {
    const token = useSelector(selectCurrentToken)
    const userData = useSelector(selectCurrentUserData)
    const location = useLocation()

    return (
        token || userData
            ? <Outlet />
            : <Navigate to="/login" state={{ from: location }} replace />
    )
}
export default RequireAuth