output "name" {
    description = "Specifies the name of the network interface"
    value       = azurerm_network_interface.nic.name
}

output "id" {
    description = "Specifies the id of the network interface"
    value       = azurerm_network_interface.nic.id
}