import { useLocation, Navigate, Outlet } from "react-router-dom"
import { useSelector } from "react-redux"
import { selectCurrentToken, selectCurrentUserData } from "./auth-slice"

const RequireAuth = ({ allowedRoles }) => {
    const token = useSelector(selectCurrentToken)
    const userData = useSelector(selectCurrentUserData)
    const location = useLocation()

    const allowed = userData?.roles?.find(roles => {
        if (allowedRoles.includes(roles)) {
            return true;
        }
    })

    return (
        (token || userData) && allowed
            ? <Outlet />
            : <Navigate to="/login" state={{ from: location }} replace />
    )
}
export default RequireAuth