// See https://aka.ms/new-console-template for more information

using System.Text;
using Amazon.SQS;
using Amazon.SQS.Model;
using Infrastructure.Constants;

Console.WriteLine("Hello, SqsConsumer!");

var cancellationTokenSource = new CancellationTokenSource();
var sqsClient = new AmazonSQSClient();
var queueUrlResponse = await sqsClient.GetQueueUrlAsync(QueueNames.UserQueueName);

var receiveMessageRequest = new ReceiveMessageRequest
{
  QueueUrl = queueUrlResponse.QueueUrl,
  AttributeNames = new List<string> { "All" },
  MessageAttributeNames = new List<string> { "All" }
};

while (!cancellationTokenSource.IsCancellationRequested)
{
  var response = await sqsClient.ReceiveMessageAsync(receiveMessageRequest, cancellationTokenSource.Token);
  var stringBuilder = new StringBuilder();
  foreach (var message in response.Messages)
  {
    stringBuilder.Append($"MessageId: {message.MessageId}, ");
    stringBuilder.Append($"MessageBody: {message.Body}");

    Console.WriteLine(stringBuilder.ToString());
    await sqsClient.DeleteMessageAsync(receiveMessageRequest.QueueUrl, message.ReceiptHandle);
  }

  await Task.Delay(3000);
}