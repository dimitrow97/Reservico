terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.50"
    }
  }
}

resource "azurerm_storage_container" "blob" {
  name                  = var.name
  storage_account_name  = var.storage_account_name
  container_access_type = var.access_type
}