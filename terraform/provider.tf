provider "aws" {
  version = "~> 2.34"
  region = var.aws_region
  shared_credentials_file = "$HOME/.aws/credentials"
  profile                 = "default"
}

resource "aws_s3_bucket" "terraform-state-storage-s3" {
  bucket = "terraform-remote-state-storage-s3"

  versioning {
    enabled = true
  }

  lifecycle {
    prevent_destroy = true
  }

  tags {
    Name = "S3 Remote Terraform State Store"
  }
}