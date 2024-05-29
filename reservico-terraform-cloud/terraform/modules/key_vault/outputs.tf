output "name" {
    description = "Specifies the name of the key vault"
    value       = azurerm_key_vault.key_vault.name
}

output "id" {
    description = "Specifies the id of the key vault"
    value       = azurerm_key_vault.key_vault.id
}