terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.50"
    }
  }
}

terraform {
  backend "azurerm" {
  }
}

provider "azurerm" {
  features {
    key_vault {
      purge_soft_delete_on_destroy    = true
      recover_soft_deleted_key_vaults = true
    }
  }
}

data "azurerm_client_config" "current" {    
}

resource "azurerm_resource_group" "rg" {
  name = "${var.resource_grp_name}-${var.env}"
  location = var.location
  tags = var.tags
}

resource "random_string" "storage_account_suffix" {
  length  = 8
  special = false
  lower   = true
  upper   = false
  numeric  = false
}

module "storage_account" {
  source                      = "./modules/storage_account"
  name                        = "${var.storage_account_name}${random_string.storage_account_suffix.result}${var.env}"
  location                    = var.location
  resource_group_name         = azurerm_resource_group.rg.name
  account_kind                = var.storage_account_kind
  account_tier                = var.storage_account_tier
  replication_type            = var.storage_account_replication_type
}

module "blob_storage" {
    source               = "./modules/blob_storage"
    name                 = "${var.blob_storage_name}-${var.env}"
    storage_account_name = module.storage_account.name
    access_type          = var.blob_storage_access_type
}

module "virtual_network" {
    source               = "./modules/virtual_network"
    resource_group_name  = azurerm_resource_group.rg.name
    location             = var.location
    name                 = "${var.vnet_name}-${var.env}"
    subnet_name          = "${var.vnet_subnet_name}-${var.env}"
    address_prefixes     = var.vnet_address_prefixes
    address_space        = var.vnet_address_space 
}

module "public_ip" {
    source               = "./modules/public_ip"
    resource_group_name  = azurerm_resource_group.rg.name
    location             = var.location
    name                 = "${var.public_ip_name}-${var.env}"
    allocation_method    = var.public_ip_allocation_method
}

module "network_interface" {
    source                        = "./modules/network_interface"
    resource_group_name           = azurerm_resource_group.rg.name
    location                      = var.location
    name                          = "${var.nic_name}-${var.env}"
    ip_config_name                = "${var.nic_name}-${var.nic_ip_config_name}-${var.env}"
    subnet_id                     = module.virtual_network.subnet_id
    public_ip_id                  = module.public_ip.Id
    private_ip_address_allocation = var.nic_private_ip_address_allocation
}

module "network_security_group" {
    source                        = "./modules/network_security_group"
    resource_group_name           = azurerm_resource_group.rg.name
    location                      = var.location
    name                          = "${var.nsg_name}-${var.env}"
    subnet_id                     = module.virtual_network.subnet_id
    whitelisted_ip_addresses      = var.nsg_whitelisted_ip_addresses
    network_interface_id          = module.network_interface.id
}

module "key_vault" {
    source                          = "./modules/key_vault"
    resource_group_name             = azurerm_resource_group.rg.name
    location                        = var.location
    name                            = "${var.kv_name}-${var.env}"
    tenant_id                       = data.azurerm_client_config.current.tenant_id
    object_id                       = data.azurerm_client_config.current.object_id
    storage_account_name            = module.storage_account.name
    virtual_network_subnet_ids      = [module.virtual_network.subnet_id]
    sku_name                        = var.kv_sku_name
    enabled_for_deployment          = var.kv_enabled_for_deployment
    enabled_for_disk_encryption     = var.kv_enabled_for_disk_encryption
    enabled_for_template_deployment = var.kv_enabled_for_template_deployment
    enable_rbac_authorization       = var.kv_enable_rbac_authorization
    purge_protection_enabled        = var.kv_purge_protection_enabled
    soft_delete_retention_days      = var.kv_soft_delete_retention_days
    bypass                          = var.kv_bypass
    default_action                  = var.kv_default_action  
    tags                            = var.tags
}