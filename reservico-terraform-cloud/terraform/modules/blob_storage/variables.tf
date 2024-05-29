variable "name" {
  description = "(Required) Specifies the name of the blob storage"
  type        = string
}

variable "storage_account_name" {
  description = "(Required) Specifies the name of the storage account"
  type        = string
}

variable "access_type" {
  description = "(Optional) Specifies the access type of the blob storage"
  default     = "blob"
  type        = string
}