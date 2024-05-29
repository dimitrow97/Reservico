variable "resource_group_name" {
  description = "(Required) Specifies the resource group name of the network interface"
  type        = string
}

variable "location" {
  description = "(Required) Specifies the location of the network interface"
  type        = string
}

variable "name" {
  type        = string
  description = "(Required) Specifies the name of the network interface"
}

variable "ip_config_name" {
  type        = string
  description = "(Required) Specifies the name of the ip configuration for the network interface"
}


variable "subnet_id" {
  type        = string
  description = "(Required) Specifies the sub network id of the ip configuration for the network interface"
}

variable "private_ip_address_allocation" {
  type        = string
  description = "(Required) Specifies the private ip address allocation of the ip configuration for the network interface"
  default     = "Dynamic"
}

variable "public_ip_id" {
  type        = string
  description = "(Required) Specifies the public ip id of the ip configuration for the network interface"
}