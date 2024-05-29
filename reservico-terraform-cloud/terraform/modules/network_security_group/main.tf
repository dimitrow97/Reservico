terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.50"
    }
  }
}

resource "azurerm_network_security_group" "network_security_grp" {
  name                = var.name  
  resource_group_name = var.resource_group_name  
  location            = var.location  
  
  security_rule {
    name                        = "Allow-Whitelisted-IPs"
    priority                    = 100
    direction                   = "Inbound"
    access                      = "Allow"
    protocol                    = "*"
    source_port_range           = "*"
    destination_port_range      = "*"
    source_address_prefixes     = var.whitelisted_ip_addresses  # IP address or CIDR range to allow
    destination_address_prefix  = "*"
  }
}

resource "azurerm_network_interface_security_group_association" "network_interface_security_group_association" {
  network_interface_id      = var.network_interface_id  
  network_security_group_id = azurerm_network_security_group.network_security_grp.id
}

resource "azurerm_subnet_network_security_group_association" "subnet_network_security_group_association" {
  subnet_id                 = var.subnet_id
  network_security_group_id = azurerm_network_security_group.network_security_grp.id
}