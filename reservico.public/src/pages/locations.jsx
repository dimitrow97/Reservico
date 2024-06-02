import Error from "@/components/common/error";
import Loader from "@/components/common/loader";
import LocationsData from "@/components/locations/locations-data"
import { useGetCategoriesQuery } from "@/features/categories/categories-api-slice"

const Locations = () => {
  const {
    data: categories,
    isLoading,
    isSuccess,
    isError,
    error
  } = useGetCategoriesQuery()

  let content;
  if (isLoading) {
    content = <Loader />;
  } else if (isSuccess) {
    content = (
      <LocationsData categories={categories} />
    )
  } else if (isError) {
    content = <Error />;
  }

  return content
}

export default Locations