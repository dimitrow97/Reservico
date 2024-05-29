output "name" {
    description = "Specifies the name of the public ip"
    value       = azurerm_public_ip.public_ip.name
}

output "Id" {
    description = "Specifies the id of the public ip"
    value       = azurerm_public_ip.public_ip.id
}
