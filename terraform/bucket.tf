resource "aws_s3_bucket" "terraform-cryptotech-marketplace-storage-bucket" {
  bucket = "terraform-cryptotech-marketplace-storage-bucket"
  acl    = "private"
  region = "eu-west-2"

  versioning {
    enabled = true
  }

  tags = {
    Name = "marketplace-storage-bucket"
  }
}