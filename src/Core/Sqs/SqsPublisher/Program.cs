// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Infrastructure.Constants;
using Infrastructure.Extensions;
using Infrastructure.ObjectMothers.User;

Console.WriteLine("Hello, Publisher!");

var sqsClient = new AmazonSQSClient();
var queueUrlResponse = await sqsClient.GetQueueUrlAsync(QueueNames.UserQueueName);
var commandCreateUser = ContractObjectMother.SimpleFakeCreateUser();
var sendMessageRequest = new SendMessageRequest
{
  QueueUrl = queueUrlResponse.QueueUrl,
  MessageBody = JsonSerializer.Serialize(ContractObjectMother.SimpleFakeCreateUser()),
  MessageAttributes = commandCreateUser.GetContractMessageAttributes()
};
var sendResponse = await sqsClient.SendMessageAsync(sendMessageRequest);
Console.WriteLine(JsonSerializer.Serialize(sendResponse));