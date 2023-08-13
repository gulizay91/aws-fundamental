# aws-fundamental
Based on [Cloud Fundamentals: AWS Services for C# Developers Course by Nick Chapsas](https://dometrain.com/course/cloud-fundamentals-aws-services-for-c-developers/)


# AWS CLI install on MacOS
```sh
> curl "https://awscli.amazonaws.com/AWSCLIV2.pkg" -o "AWSCLIV2.pkg"
> sudo installer -pkg AWSCLIV2.pkg -target /
> aws --version
```
# Configure AWS CLI
```sh
> aws configure
> aws configure list-profiles
> aws configure set region us-west-1 --profile <profileName/>
> aws configure get region --profile <profileName/>
> aws configure get aws_secret_access_key --profile <profileName/>
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
### Install Amazon.Lambda.Tools Global Tools if not already installed.
```
> dotnet tool install -g Amazon.Lambda.Tools
```
### If already installed check if new version is available.
```
> dotnet tool update -g Amazon.Lambda.Tools
```
### Install Lambda additional templates If you need, it is available.
```
> dotnet new -i Amazon.Lambda.Templates
```
# [AWS Lambda Functions](https://docs.aws.amazon.com/lambda/latest/dg/csharp-package-cli.html)
### List lambda functions
```
> aws lambda list-functions
```

### Invoke function & get content on aws lambda node.js
```
> aws lambda invoke --function-name <LambdaFunctionName> --cli-binary-format raw-in-base64-out --payload '{ ""Hello"": ""From Console"" }' response.json
> Get-Content .\response.json
```

### Deploy AWS Lambda function with Dotnet Project: SimpleLambda
```
> cd "../<projectFolder>"
> dotnet lambda deploy-function SimpleLambda
```
### Invoke AWS Lambda function
```
>  dotnet lambda invoke-function SimpleLambda --payload '{ "input": "lambda" }'
```

### Deploy AWS Lambda Serverless with Dotnet Project: SimpleLambdaServerless
```
> cd "../<projectFolder>"
> dotnet lambda deploy-serverless SimpleLambdaServerless
```

### Delete AWS Lambda Serverless with Dotnet Project: SimpleLambdaServerless
```
> cd "../<projectFolder>"
> dotnet lambda delete-serverless SimpleLambdaServerless
```
### [Install AWS Lambda TestTool](https://github.com/aws/aws-lambda-dotnet/blob/master/Tools/LambdaTestTool/README.md#installing-and-running)
```
> dotnet tool install -g Amazon.Lambda.TestTool-6.0
```
### Build dotnet and run test tool
```
> cd "../<projectFolder>"
> dotnet build
> dotnet lambda-test-tool-6.0
```
### Deploy AWS Sqs Lambda with Dotnet Project: SimpleSqsLambda
```
> cd "../<projectFolder>"
> dotnet lambda deploy-function SimpleSqsLambda
```


