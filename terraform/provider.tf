provider "aws" {
  version = "~> 2.34"
  region = var.aws_region
  shared_credentials_file = "$HOME/.aws/credentials"
  profile                 = "default"
}