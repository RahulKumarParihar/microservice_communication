using Amazon.SQS;
using Amazon.SQS.Model;
using MicroServiceCommunication.MessageBrokers.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroServiceCommunication.MessageBrokers.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    private readonly AWSConfigurations _aWSConfigurations;

    public MessageController(IConfiguration configuration)
    {
        _aWSConfigurations = configuration.GetSection("AWSConfigurations").Get<AWSConfigurations>();
    }

    [HttpPost]
    public async Task<IActionResult> PostMessage(MessageRequest request)
    {
        var region = Amazon.RegionEndpoint.GetBySystemName(_aWSConfigurations.Region);

        if (region is null)
            return BadRequest("Region must be specified, check AWSConfigurations");

        using IAmazonSQS sqs = new AmazonSQSClient(_aWSConfigurations.AccessKeyId, _aWSConfigurations.SecretAccessKey, region);
        var sqsRequest = new CreateQueueRequest
        {
            QueueName = "request_queue",
            Attributes = new Dictionary<string, string>()
                {
                    { "MessageRetentionPeriod", "600" }
                }
        };

        var createQueueResponse = await sqs.CreateQueueAsync(sqsRequest)
        .ConfigureAwait(false);

        var response = await sqs.SendMessageAsync(createQueueResponse.QueueUrl, request.Message)
            .ConfigureAwait(false);

        return Ok();
    }
}
