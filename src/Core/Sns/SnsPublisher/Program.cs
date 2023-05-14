// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Infrastructure.Constants;
using Infrastructure.Extensions;
using Infrastructure.ObjectMothers.User;

Console.WriteLine("Hello, SnsPublisher! This publisher' messages consume by SqsConsumer");

var snsClient = new AmazonSimpleNotificationServiceClient();
var commandCreateUser = ContractObjectMother.SimpleFakeCreateUser();

var topicArnResponse = await snsClient.FindTopicAsync(QueueNames.UserQueueName);

var publishRequest = new PublishRequest
{
  TopicArn = topicArnResponse.TopicArn,
  Message = JsonSerializer.Serialize(commandCreateUser),
  MessageAttributes = commandCreateUser.GetSnsContractMessageAttributes()
};

var response = await snsClient.PublishAsync(publishRequest);

Console.WriteLine(JsonSerializer.Serialize(response));