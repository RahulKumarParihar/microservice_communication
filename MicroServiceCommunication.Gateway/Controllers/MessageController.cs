using Amazon.SQS;
using Amazon.SQS.Model;
using MicroServiceCommunication.Gateway.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroServiceCommunication.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class MessagesController : ControllerBase
{
    private readonly AWSConfigurations _aWSConfigurations;

    public MessagesController(IConfiguration configuration)
    {
        _aWSConfigurations = configuration.GetSection("AWSConfigurations").Get<AWSConfigurations>();
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IActionResult> Get()
    {
        var region = Amazon.RegionEndpoint.GetBySystemName(_aWSConfigurations.Region);

        if (region is null)
            return BadRequest("Region must be specified, check AWSConfigurations");

        using IAmazonSQS sqs = new AmazonSQSClient(_aWSConfigurations.AccessKeyId, _aWSConfigurations.SecretAccessKey, region);

        var queueUrlResponse = await sqs.GetQueueUrlAsync("request_queue")
            .ConfigureAwait(false);

        var sqsRequest = new ReceiveMessageRequest
        {
            QueueUrl = queueUrlResponse.QueueUrl,
            MaxNumberOfMessages = 1,
            WaitTimeSeconds = 10
        };

        var response = await sqs.ReceiveMessageAsync(sqsRequest)
            .ConfigureAwait(false);

        return Ok(response.Messages.FirstOrDefault());
    }
}
