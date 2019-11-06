# Set up CloudWatch group and log stream and retain logs for 30 days
resource "aws_cloudwatch_log_group" "marketplace_log_group" {
  name              = "/ecs/marketplace-app"
  retention_in_days = 30

  tags = {
    Name = "marketplace-log-group"
  }
}

resource "aws_cloudwatch_log_stream" "marketplace_log_stream" {
  name           = "marketplace-log-stream"
  log_group_name = aws_cloudwatch_log_group.marketplace_log_group.name
}