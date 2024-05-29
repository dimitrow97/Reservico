variable "resource_group_name" {
  description = "(Required) Specifies the resource group name of the public ip"
  type        = string
}

variable "location" {
  description = "(Required) Specifies the location of the public ip"
  type        = string
}

variable "name" {
  type        = string
  description = "(Required) Specifies the name of the public ip"
}

variable "allocation_method" {
  type        = string
  description = "(Required) Specifies the allocation method of the public ip"
  default     = "Dynamic"
}