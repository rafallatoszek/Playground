using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace Main.KafkaProducer;

[ApiController]
[Route("[controller]")]
public class KafkaProducerController : ControllerBase
{
    private readonly ILogger<KafkaProducerController> _logger;

    public KafkaProducerController(ILogger<KafkaProducerController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "kafka-producer")]
    public async Task<IActionResult> Get()
    {
        var config = new ProducerConfig { BootstrapServers = "kafka:9092" };
        using var producer = new ProducerBuilder<Null, string>(config).Build();
        await producer.ProduceAsync("test-topic", new Message<Null, string> { Value="a new message" });
        return Ok();
    }
}
