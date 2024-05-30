import * as React from "react"

import {
  NavigationMenu,
  NavigationMenuItem,
  NavigationMenuLink,
  NavigationMenuList,
  navigationMenuTriggerStyle,
} from "@/components/ui/navigation-menu"

const Navigation = () => {
  return (
    <NavigationMenu>
      <NavigationMenuList>        
        <NavigationMenuItem>
          <a href="/locations">
            <span href="/locations" className="group border-white border-2 inline-flex h-10 w-max items-center justify-center rounded-md bg-transparent px-4 py-2 text-sm text-white font-medium transition-colors hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground focus:outline-none disabled:pointer-events-none disabled:opacity-50 data-[active]:bg-accent/50 data-[state=open]:bg-accent/50">
              Locations
            </span>
          </a>
        </NavigationMenuItem>
      </NavigationMenuList>
    </NavigationMenu>
  )
}

export default Navigation