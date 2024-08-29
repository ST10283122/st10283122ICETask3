using Azure.Storage.Queues;
using Azure.Storage.Files.Shares;
using System;
using System.IO;
using System.Text;

namespace MessageProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            // Azure Storage connection strings
            string queueConnectionString = "";
            string fileConnectionString = "";
            string queueName = "";
            string shareName = "";
            string fileName = "";

            // Create queue client
            QueueClient queueClient = new QueueClient(queueConnectionString, queueName);

            // Create file share client
            ShareClient shareClient = new ShareClient(fileConnectionString, shareName);
            shareClient.CreateIfNotExists();
            ShareDirectoryClient directoryClient = shareClient.GetRootDirectoryClient();
            ShareFileClient fileClient = directoryClient.GetFileClient(fileName);

            StringBuilder fileContent = new StringBuilder();

            // Process messages from the queue
            while (true)
            {
                var message = queueClient.ReceiveMessage();
                if (message.Value == null) break;

                // Add message content to the file content
                fileContent.AppendLine(message.Value.MessageText);

                // Delete processed message
                queueClient.DeleteMessage(message.Value.MessageId, message.Value.PopReceipt);
            }

            // Write to Azure File Storage
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent.ToString())))
            {
                fileClient.Create(stream.Length);
                fileClient.Upload(stream);
            }

            Console.WriteLine("Messages processed and saved to Azure File Storage.");
        }
    }
}
