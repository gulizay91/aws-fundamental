# aws-fundamental
Based on [Cloud Fundamentals: AWS Services for C# Developers Course by Nick Chapsas](https://dometrain.com/course/cloud-fundamentals-aws-services-for-c-developers/)

# AWS SQS Queues
```sh
arn:aws:sqs:<Region>:<AccountId>:users
arn:aws:sqs:<Region>:<AccountId>:users-dlq
```

# AWS SQS Policy
```sh
{
  "Version": "2012-10-17",
  "Id": "__default_policy_ID",
  "Statement": [
    {
      "Sid": "__owner_statement",
      "Effect": "Allow",
      "Principal": {
        "AWS": "arn:aws:iam::<AccountId>:root"
      },
      "Action": "SQS:*",
      "Resource": "arn:aws:sqs:<Region>:<AccountId>:users"
    },
    {
      "Effect": "Allow",
      "Principal": {
        "Service": "sns.amazonaws.com"
      },
      "Action": "sqs:SendMessage",
      "Resource": "arn:aws:sqs:<Region>:<AccountId>:users",
      "Condition": {
        "ArnEquals": {
          "aws:SourceArn": "arn:aws:sns:<Region>:<AccountId>:users"
        }
      }
    }
  ]
}
```

# AWS SNS Topics & Subscription
```sh
arn:aws:sns:<Region>:<AccountId>:users -> Topics
arn:aws:sqs:<Region>:<AccountId>:users -> Subscription Endpoint
```

# AWS SNS Policy
```sh
{
  "Version": "2008-10-17",
  "Id": "__default_policy_ID",
  "Statement": [
    {
      "Sid": "__default_statement_ID",
      "Effect": "Allow",
      "Principal": {
        "AWS": "*"
      },
      "Action": [
        "SNS:GetTopicAttributes",
        "SNS:SetTopicAttributes",
        "SNS:AddPermission",
        "SNS:RemovePermission",
        "SNS:DeleteTopic",
        "SNS:Subscribe",
        "SNS:ListSubscriptionsByTopic",
        "SNS:Publish"
      ],
      "Resource": "arn:aws:sns:<Region>:<AccountId>:users",
      "Condition": {
        "StringEquals": {
          "AWS:SourceOwner": "<AccountId>"
        }
      }
    }
  ]
}
```
