import { useGetDashboardQuery } from "../features/dashboard/dashboard-api-slice"
import { useSelector } from "react-redux"
import { selectCurrentClient } from "../features/auth/auth-slice"
import Loader from "../components/common/loader"
import DashboardCards from "../components/dashboard/dashboard-cards"
import DashboardTable from "../components/dashboard/dashboard-table"

const Home = () => {
  const currentClient = useSelector(selectCurrentClient)

  const {
    data: dashboard,
    isLoading,
    isSuccess,
    isError,
    error
  } = useGetDashboardQuery(currentClient)

  let content;
  if (isLoading) {
    content = <Loader />;
  } else if (isSuccess) {
    content = (
      <main className="flex flex-1 flex-col gap-4 p-4 md:gap-8 md:p-8">
        <DashboardCards data={dashboard.data} />
        <DashboardTable data={dashboard.data.lastFiveReservations} />
      </main>
    )
  } else if (isError) {
    content = <p>{JSON.stringify(error)}</p>;
  }

  return content
}

export default Home