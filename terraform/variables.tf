variable "aws_region" {
  description = "The AWS region things are created in"
  default     = "eu-west-2"
}

variable "ecs_task_execution_role_name" {
  description = "ECS task execution role name"
  default     = "task_execution_role"
}

//variable "ecs_auto_scale_role_name" {
//  description = "ECS auto scale role Name"
//  default = "myEcsAutoScaleRole"
//}

variable "az_count" {
  description = "Number of AZs to cover in a given region"
  default     = "2"
}

variable "image_tag" {
  description = "Docker image to deploy to the cluster"
  type        = string
}

variable "app_port" {
  description = "Port exposed by the docker image to redirect traffic to"
  type        = number
  default     = 5000
}

variable "app_count" {
  description = "Number of docker containers to run"
  default     = 1
}

variable "health_check_path" {
  default = "/"
}

variable "fargate_cpu" {
  description = "Fargate instance CPU units to provision (1 vCPU = 1024 CPU units)"
  type        = number
  default     = 1024
}

variable "fargate_memory" {
  description = "Fargate instance memory to provision (in MiB)"
  type        = number
  default     = 2048
}

variable "airtable_table_id" {
  description = "Airtable table id"
  type        = string
}

variable "airtable_api_key" {
  description = "Airtable api key"
  type        = string
}

variable "sentry_dsn" {
  description = "Sentry DSN"
  type        = string
}