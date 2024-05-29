output "vnet_name" {
    description = "Specifies the name of the virtual network"
    value       = azurerm_virtual_network.virtual_network.name
}

output "subnet_name" {
    description = "Specifies the name of the sub network"
    value       = azurerm_subnet.subnet.name
}

output "vnet_id" {
    description = "Specifies the id of the virtual network"
    value       = azurerm_virtual_network.virtual_network.id
}

output "subnet_id" {
    description = "Specifies the id of the sub network"
    value       = azurerm_subnet.subnet.id
}