using Azure.Storage.Queues;
using System;

namespace FibonacciSequenceGenerator
{
    class Program
    {
        static void Main(string[] args)
        {

            string connectionString = "Your_Azure_Queue_Storage_Connection_String";
            string queueName = "fibonaccisequence";


            QueueClient queueClient = new QueueClient(connectionString, queueName);

            queueClient.CreateIfNotExists();

            int a = 0, b = 1;
            while (a <= 233)
            {
                Console.WriteLine(a);

                queueClient.SendMessage(a.ToString());

                int temp = a;
                a = b;
                b = temp + b;
            }

            Console.WriteLine("Fibonacci sequence generated and stored in Azure Queue Storage.");
        }
    }
}
