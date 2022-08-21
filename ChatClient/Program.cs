#region usings
using ChatClient;
using MesssageServiceReference;
#endregion

var messageServiceClient = new MessageServiceClient("http://localhost:5195", new HttpClient());
var clientId = messageServiceClient.ConnectAsync("Test").Result;

var messageClient = new MessageClient(clientId);

while (true)
{
    messageClient.SendMessageAsync(1002, "Hello!");
    Console.ReadKey();
}