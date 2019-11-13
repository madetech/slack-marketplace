[
  {
    "name": "marketplace-app",
    "image": "${app_image}",
    "cpu": ${fargate_cpu},
  "memory": ${fargate_memory},
  "networkMode": "awsvpc",
  "logConfiguration": {
  "logDriver": "awslogs",
  "options": {
    "awslogs-group": "/ecs/marketplace-app",
    "awslogs-region": "${aws_region}",
    "awslogs-stream-prefix": "ecs"
  }
},
  "portMappings": [
  {
    "containerPort": ${app_port},
  "hostPort": ${app_port}
}
],
  "environment": [
  {
    "name": "PORT",
    "value": "5000"
  },
  {
    "name": "AIRTABLE_API_KEY",
    "value": "keyLzSMyLBNl3BspM"
  },

  {
    "name": "AIRTABLE_TABLE_ID",
    "value": "apppGaPfIFW8nmf4T"
  },
  {
    "name": "AIRTABLE_URL",
    "value": "https://api.airtable.com/"
  }
]
  }
] 