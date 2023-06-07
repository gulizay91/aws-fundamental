// See https://aka.ms/new-console-template for more information


using System.Text;
using Amazon.S3;
using Amazon.S3.Model;


const string bucketName = "themoonstudio-root";
const string key = "files/movies.csv";
const string contentType = "text/csv";

var s3Client = new AmazonS3Client();

await MainMenu();

async Task MainMenu()
{
  string? inputKey;
  do
  {
    Menu();
    inputKey = Console.ReadLine();
    switch (inputKey)
    {
      case "1":
        await UploadFileToS3(bucketName, key, contentType);
        break;
      case "2":
        await DownloadFileToS3(bucketName, key);
        break;
      case "3":
        await GetPreSignedUrlFromS3(bucketName, key);
        break;
      default:
        Console.WriteLine("choose wisely!");
        break;
    }
    
  } while (inputKey?.ToLower() != "q");
  Console.WriteLine("Bye");
  Environment.Exit(0);
}

void Menu()
{
  Console.WriteLine("************ S3 Menu ************");
  Console.WriteLine("exit : q || Q ");
  Console.WriteLine("1- Upload csv to S3");
  Console.WriteLine("2- Download csv from S3");
  Console.WriteLine("3- GetPreSignedUrlFromS3 csv from S3");
}


async Task UploadFileToS3(string bucketName, string key, string contentType)
{
  await using var inputStream = new FileStream("./movies.csv", FileMode.Open, FileAccess.Read);

  var putObjectRequest = new PutObjectRequest
  {
    BucketName = bucketName,
    Key = key,
    ContentType = contentType,
    InputStream = inputStream
  };

  await s3Client.PutObjectAsync(putObjectRequest);
}

async Task DownloadFileToS3(string bucketName, string key)
{
  var getObjectRequest = new GetObjectRequest
  {
    BucketName = bucketName,
    Key = key
  };

  var response = await s3Client.GetObjectAsync(getObjectRequest);

  using var memoryStream = new MemoryStream();
  response.ResponseStream.CopyTo(memoryStream);

  var text = Encoding.Default.GetString(memoryStream.ToArray());

  Console.WriteLine(text);
}


Task GetPreSignedUrlFromS3(string bucketName, string key)
{
  // Create a CopyObject request
  var request = new GetPreSignedUrlRequest
  {
    BucketName = bucketName,
    Key = key,
    Expires = DateTime.Now.AddMinutes(5)
  };

  // Get path for request
  var path = s3Client.GetPreSignedURL(request);
  Console.WriteLine(path);
  return Task.CompletedTask;
}