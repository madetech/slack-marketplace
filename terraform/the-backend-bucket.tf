terraform {
  backend "s3" {
    encrypt        = true
    bucket         = "terraform-cryptotech-marketplace-storage-bucket"
    region         = "eu-west-2"
    key            = "terraform.tfstate"
    dynamodb_table = "terraform-cryptotech-state-lock-dynamo"
  }
}