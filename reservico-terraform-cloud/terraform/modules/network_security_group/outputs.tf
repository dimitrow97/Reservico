output "name" {
    description = "Specifies the name of the network security group"
    value       = azurerm_network_security_group.network_security_grp.name
}

output "id" {
    description = "Specifies the id of the network security group"
    value       = azurerm_network_security_group.network_security_grp.id
}