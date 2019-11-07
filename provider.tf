provider "aws" {
  version = "~> 2.34"
  region = var.aws_region
  skip_region_validation = true
  shared_credentials_file = "~/.aws/credentials"
  profile = "default"
}

resource "aws_s3_bucket" "terraform-state-storage-s3-3456789-marketplace-thing" {
  bucket = "terraform-marketplace-storage-bucket-3334444555-adjfhfdjsalkjd"

  versioning {
    enabled = true
  }

  lifecycle {
    prevent_destroy = true
  }

  tags {
    Name = "S3 Remote Terraform State Store 45678888"
  }
}

terraform {
  backend "s3" {
encrypt = true
bucket = "terraform-marketplace-storage-bucket-3334444555-adjfhfdjsalkjd"
region = "eu-west-2"
key = "terraform.tfstate"
}
}