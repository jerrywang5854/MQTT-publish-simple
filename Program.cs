using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Configuration constants for MQTT connection
        int clientCount = 100; // Total number of MQTT clients to simulate
        int messageCount = 5; // Number of messages each client will send
        int delayBetweenMessages = 1000; // Delay between messages in milliseconds (1 second)
        string brokerIP = "yourbrokerIP";
        string port = "yourport";
        string username = "yourname";
        string userpassword = "yourpassword";

        // Array to hold tasks for each client operation
        Task[] clientTasks = new Task[clientCount];

        // Loop to initialize and run each client in its own task
        for (int i = 0; i < clientCount; i++)
        {
            int clientId = i;
            int topicId = i % 1000; // Modulo operation to generate topic IDs
            clientTasks[i] = Task.Run(async () =>
            {
                try
                {
                    // Create MQTT client using the factory pattern
                    var client = new MqttFactory().CreateMqttClient();
                    // Build connection options including WebSocket protocol and MQTT version 5
                    var options = new MqttClientOptionsBuilder()
                        .WithWebSocketServer($"{brokerIP}:{port}/mqtt")
                        .WithClientId($"Client_{clientId}")
                        .WithCredentials(username, userpassword)
                        .WithCleanSession(true)
                        .WithCommunicationTimeout(TimeSpan.FromSeconds(30))
                        .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500)
                        .Build();

                    // Connect the client to the broker with no cancellation token
                    await client.ConnectAsync(options, CancellationToken.None);

                    // Send messages in a loop for the designated message count
                    for (int j = 0; j < messageCount; j++)
                    {
                        string messagePayload = $"{topicId}-{j}"; // Payload includes topic ID and message number
                        var message = new MqttApplicationMessageBuilder()
                            .WithTopic($"topic_test/{topicId}") // Topic based on topic ID
                            .WithPayload(messagePayload)
                            .WithExactlyOnceQoS() // Set QoS to ExactlyOnce for reliable delivery
                            .Build();

                        // Publish the message asynchronously
                        await client.PublishAsync(message, CancellationToken.None);
                        Console.WriteLine($"Client {clientId} sent message {j + 1} on topic/{topicId}");
                        // Wait for the specified delay before sending the next message
                        await Task.Delay(delayBetweenMessages);
                    }

                    // Disconnect the client from the broker
                    await client.DisconnectAsync();
                }
                catch (Exception ex)
                {
                    // Log any exceptions that occur during client operation
                    Console.WriteLine($"Error in client {clientId}: {ex.Message}");
                }
            });
        }

        // Wait for all client tasks to complete
        await Task.WhenAll(clientTasks);
        Console.WriteLine("All clients have sent their messages. Exiting program.");
    }
}
