output "name" {
    description = "Specifies the name of the blob storage"
    value       = azurerm_storage_container.blob.name
}

output "id" {
    description = "Specifies the id of the blob storage"
    value       = azurerm_storage_container.blob.id
}