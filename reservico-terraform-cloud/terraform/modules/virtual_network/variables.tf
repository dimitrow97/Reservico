variable "resource_group_name" {
  description = "(Required) Specifies the resource group name of the virtual network"
  type        = string
}

variable "location" {
  description = "(Required) Specifies the location of the virtual network"
  type        = string
}

variable "name" {
  type        = string
  description = "(Required) Specifies the name of the virtual network"
}

variable "subnet_name" {
  type        = string
  description = "(Required) Specifies the name of the sub network"
}

variable "address_space" {
  type        = list(string)
  description = "(Required) Specifies the address space of the virtual network"
}

variable "address_prefixes" {
  type        = list(string)
  description = "(Required) Specifies the address prefixes of the sub network"
}