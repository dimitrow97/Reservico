import { useState } from "react"

import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectLabel,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select"
import { useDispatch, useSelector } from "react-redux"
import { apiSlice } from "../../app/api/api-slice"
import { selectCurrentClient } from "../../features/auth/auth-slice"
import { useGetClientsForUserQuery, useUpdateUserSelectedClientMutation } from "../../features/users/users-api-slice"
import Loader from "./loader"

export function ClientSelect() {
  const {
    data: clients,
    error,
    isError,
    isLoading,
    isSuccess } = useGetClientsForUserQuery()

  const dispatch = useDispatch()
  const currentClient = useSelector(selectCurrentClient)
  const [selectedOption, setSelectedOption] = useState(currentClient)
  const [updateSelectedClient] = useUpdateUserSelectedClientMutation()

  const handleChange = async (value) => {
    const request = {
      clientId: value
    }

    try {
      const response = await updateSelectedClient(request).unwrap()

      if (response.isSuccess) {
        dispatch(apiSlice.util.invalidateTags(["client-users"]))
        dispatch(setUserSelectedClient(request))
        setSelectedOption(value)
      }
    } catch (err) {
      console.log(err);
    }
  }

  let content;
  if (isLoading) {
    content = <Loader />;
  } else if (isSuccess) {
    content = (
      <Select
        value={selectedOption}
        defaultValue={selectedOption}
        onValueChange={handleChange}
        className="flex item-center justify-between gap-2 border rounded-[8px] p-2">
        <SelectTrigger className="gap-2 border rounded-[8px] p-2">
          <SelectValue placeholder="Select a Client" />
        </SelectTrigger>
        <SelectContent>
          <SelectGroup>
            <SelectLabel>Clients</SelectLabel>
            {clients.data.map((client, key) => (
              <SelectItem key={key} value={client.clientId}>
                {client.clientName}
              </SelectItem>
            ))}
          </SelectGroup>
        </SelectContent>
      </Select>
    )
  } else if (isError) {
    content = <p>{JSON.stringify(error)}</p>;
  }

  return content
}

export default ClientSelect