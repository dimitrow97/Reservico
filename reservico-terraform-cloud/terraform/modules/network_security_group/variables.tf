variable "resource_group_name" {
  description = "(Required) Specifies the resource group name of the network security group"
  type        = string
}

variable "location" {
  description = "(Required) Specifies the location of the network security group"
  type        = string
}

variable "name" {
  description = "(Required) Specifies the name of the network security group"
  type        = string
}

variable "whitelisted_ip_addresses" {
  description = "(Required) Specifies the list of whitelisted IP Addresses for the network security group"
  type        = list(string)
}

variable "network_interface_id" {
  description = "(Required) Specifies the network interface id for the network interface security group association"
  type        = string
}

variable "subnet_id" {
  description = "(Required) Specifies the sub network id for the subnet network security group association"
  type        = string
}