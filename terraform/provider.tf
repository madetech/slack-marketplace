provider "aws" {
  version                 = "~> 2.34"
  region                  = var.aws_region
  skip_region_validation  = true
  shared_credentials_file = "~/.aws/credentials"
  profile                 = "default"
}