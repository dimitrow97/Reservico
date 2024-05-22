import UserInfo from './user-info'
import { Link } from 'react-router-dom';

import {
    Calculator,
    Calendar,
    CreditCard,
    Settings,
    Smile,
    User,
    MapPin
} from "lucide-react"
   
import {
  Command,
  CommandGroup,
  CommandItem,
  CommandList
} from "@/components/ui/command"

const menuList = [
    {
        group: "General",
        items: [
            {
                icon: <Smile className="mr-2 h-4 w-4" />,
                link: "/clients",
                text: "Clients"
            },
            {
                icon: <MapPin className="mr-2 h-4 w-4" />,
                link: "/",
                text: "Locations"
            },
            {
                icon: <Calendar className="mr-2 h-4 w-4" />,
                link: "/",
                text: "Reservations"
            },
            {
                icon: <Settings className="mr-2 h-4 w-4" />,
                link: "/",
                text: "Categories"
            },
            {
                icon: <User className="mr-2 h-4 w-4" />,
                link: "/",
                text: "Users"
            },
        ]
    }
]

const Sidebar = () => {
    return (
        <div className="fixed flex flex-col gap-4 w-[300px] min-w-[300px] border-r min-h-screen p-4">
            <div>
                <UserInfo />
            </div>
            <div className="grow">
                <Command style={{ overflow: 'visible' }}>                    
                    <CommandList style={{ overflow: 'visible' }}>
                     {menuList.map((menu, key) => (
                      <CommandGroup key={key} heading={menu.group}>
                        {menu.items.map((option, optionKey) =>
                            <CommandItem key={optionKey} className="flex gap-2">
                                {option.icon}
                                <Link to={option.link}>{option.text}</Link>
                            </CommandItem>
                        )}
                      </CommandGroup>
                      ))}
                    </CommandList>
                </Command>
            </div>
        </div>
    )
}

export default Sidebar