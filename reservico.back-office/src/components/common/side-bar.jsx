import UserInfo from './user-info'
import { Link } from 'react-router-dom';

import {
    Home,
    Calendar,
    Settings,
    Smile,
    User,
    MapPin
} from "lucide-react"

import {
    Command,
    CommandGroup,
    CommandItem,
    CommandList,
    CommandSeparator 
} from "@/components/ui/command"

const menuList = [
    {
        group: "General",
        items: [
            {
                icon: <Home className="mr-2 h-4 w-4" />,
                link: "/home",
                text: "Home"
            },
        ],      
    },
    {
        group: "Clients & Users",
        items: [
            {
                icon: <Smile className="mr-2 h-4 w-4" />,
                link: "/clients",
                text: "Clients"
            },
            {
                icon: <User className="mr-2 h-4 w-4" />,
                link: "/users",
                text: "Users"
            },
        ],
    },
    {
        group: "Entities",
        items: [
            {
                icon: <Calendar className="mr-2 h-4 w-4" />,
                link: "/reservations",
                text: "Reservations"
            },
            {
                icon: <Settings className="mr-2 h-4 w-4" />,
                link: "/categories",
                text: "Categories"
            },
        ]
    }
]

const Sidebar = () => {
    return (
        <div className="fixed flex flex-col gap-4 w-[300px] min-w-[300px] p-4 min-h-screen">
            <div>
                <UserInfo />
            </div>
            <div className="grow">
                <Command style={{ overflow: 'visible' }}>
                    <CommandList style={{ overflow: 'visible' }}>
                        {menuList.map((menu, key) => (
                            <div key={key}>
                                <CommandGroup key={key} heading={menu.group}>
                                    {menu.items.map((option, optionKey) =>
                                        <CommandItem key={optionKey} className="flex gap-2">
                                            {option.icon}
                                            <Link to={option.link}>{option.text}</Link>
                                        </CommandItem>
                                    )}
                                </CommandGroup>
                                <CommandSeparator />
                            </div>
                        ))}
                    </CommandList>
                </Command>
            </div>
        </div>
    )
}

export default Sidebar