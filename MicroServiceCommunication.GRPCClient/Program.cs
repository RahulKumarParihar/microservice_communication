using Grpc.Net.Client;
using MicorServiceCommunication.GRPCClient;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        string name = args.Length > 0 && String.IsNullOrWhiteSpace(args[0])
        ? "GreeterClient"
        : args[0];

        using var channel = GrpcChannel.ForAddress("https://localhost:7082");
        var client = new Greeter.GreeterClient(channel);
        var reply = await client.SayHelloAsync(
                          new HelloRequest { Name = name });
        Console.WriteLine("Greeting: " + reply.Message);
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}