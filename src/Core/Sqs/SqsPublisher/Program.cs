﻿// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Infrastructure.Constants;
using Infrastructure.Extensions;
using Infrastructure.ObjectMothers.User;

Console.WriteLine("Hello, SqsPublisher!");

var sqsClient = new AmazonSQSClient();

var queueUrlResponse = await sqsClient.GetQueueUrlAsync(QueueNames.SqsTestQueueName);
var commandCreateUser = ContractObjectMother.SimpleFakeCreateUser();
var sendMessageRequest = new SendMessageRequest
{
  QueueUrl = queueUrlResponse.QueueUrl,
  MessageBody = JsonSerializer.Serialize(ContractObjectMother.SimpleFakeCreateUser()),
  MessageAttributes = commandCreateUser.GetSqsContractMessageAttributes()
};
var sendResponse = await sqsClient.SendMessageAsync(sendMessageRequest);
Console.WriteLine(JsonSerializer.Serialize(sendResponse));