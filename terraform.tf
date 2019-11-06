terraform {
  backend s3 {
encrypt = true
bucket = "terraform-remote-state-storage-s3"
region = eu-west-2
key = "terraform.tfstate"
}
}