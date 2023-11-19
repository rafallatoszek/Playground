using Confluent.Kafka;

public class KafkaConsumer : BackgroundService
{
    private readonly string _topic = "test-topic";
    private readonly IConsumer<Null, string> _kafkaConsumer;

    public KafkaConsumer()
    {
        var consumerConfig = new ConsumerConfig()
        {
            BootstrapServers = "kafka:9092",
            GroupId = "test-group"
        };

        _kafkaConsumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() => StartConsumerLoop(stoppingToken), stoppingToken);
    }

    private void StartConsumerLoop(CancellationToken cancellationToken)
    {
        _kafkaConsumer.Subscribe(_topic);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _kafkaConsumer.Consume(cancellationToken);
                Console.WriteLine("Offset: " + consumeResult.Offset);
                Handle(consumeResult.Message.Value);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (ConsumeException e)
            {
                Console.WriteLine($"Consume error: {e.Error.Reason}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unexpected error: {e}");
                break;
            }
        }
    }

    private void Handle(string message) {
        Console.WriteLine("Received: " + message);
    }

    public override void Dispose()
    {
        _kafkaConsumer.Close();
        _kafkaConsumer.Dispose();
        base.Dispose();
    }
}