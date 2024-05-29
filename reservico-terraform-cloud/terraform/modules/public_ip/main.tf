terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.50"
    }
  }
}

resource "azurerm_public_ip" "public_ip" {
  name                = var.name  
  resource_group_name = var.resource_group_name 
  location            = var.location
  allocation_method   = var.allocation_method
}