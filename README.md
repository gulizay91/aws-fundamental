# aws-fundamental
Based on [Cloud Fundamentals: AWS Services for C# Developers Course by Nick Chapsas](https://dometrain.com/course/cloud-fundamentals-aws-services-for-c-developers/)


# AWS CLI install on MacOS
```sh
> curl "https://awscli.amazonaws.com/AWSCLIV2.pkg" -o "AWSCLIV2.pkg"
> sudo installer -pkg AWSCLIV2.pkg -target /
> aws --version
```

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

# AWS SNS Topic Subscription Filter Policy
### Message attributes filter policy scope
If we use this policy, only messages with this attribute will receive.
```sh
{
  "MessageType": [
    "CreateUser"
  ]
}
```

# Dotnet AWS tool
```
> dotnet tool install -g Amazon.Lambda.Tools
> dotnet new -i Amazon.Lambda.Templates
```
# AWS Lambda Functions
### List lambda functions
```
> aws lambda list-functions
```

### Invoke function & get content on aws lambda node.js
```
> aws lambda invoke --function-name <LambdaFunctionName> --cli-binary-format raw-in-base64-out --payload '{ ""Hello"": ""From Console"" }' response.json
> Get-Content .\response.json
```

### Deploy function to AWS Lambda Dotnet
```
> cd "../<projectFolder>"
> dotnet lambda deploy-function <LambdaFunctionName>
```
### Deploy function to AWS Lambda Dotnet
```
>  dotnet lambda invoke-function SimpleLambda --payload '{ "input": "lambda" }'
```


