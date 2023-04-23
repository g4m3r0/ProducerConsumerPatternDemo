namespace ProducerConsumerPatternDemo;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        // Create a channel to communicate between the producer and consumer.
        var channel = Channel.CreateUnbounded<int>();

        var producerTask = ProduceAsync(channel.Writer);
        var consumerTask = ConsumeAsync(channel.Reader);

        // Wait for both tasks to complete.
        await Task.WhenAll(producerTask, consumerTask);
    }


    public static async Task ProduceAsync(ChannelWriter<int> channelWriter)
    {
        var random = new Random();

        while (true)
        {
            // Produce random numbers between 0 and 10000.
            var randomNumber = random.Next(0, 10000);
            await channelWriter.WriteAsync(randomNumber);

            // Wait for a random amount of time.
            await Task.Delay(randomNumber);
        }
    }

    public static async Task ConsumeAsync(ChannelReader<int> channelReader)
    {
        // Read from the channel and use them.
        await foreach (var number in channelReader.ReadAllAsync())
        {
            await Task.Delay(number);
            Console.WriteLine($"Consumer waited for {number}ms.");
        }
    }
}
