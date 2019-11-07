resource "aws_dynamodb_table" "dynamodb-cryptotech-terraform-state-lock" {
  name           = "terraform-cryptotech-state-lock-dynamo"
  hash_key       = "LockID"
  read_capacity  = 20
  write_capacity = 20
  attribute {
    name = "LockID"
    type = "S"
  }
  tags = {
    Name = "DynamoDB Terraform State Lock Table"
  }
}