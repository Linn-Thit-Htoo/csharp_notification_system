using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Newtonsoft.Json;

namespace notification_system.notification.Extensions;

public static class Extension
{
    public static string ToJson(this object obj) =>
        JsonConvert.SerializeObject(obj, Formatting.Indented);

    public static T ToObject<T>(this string jsonStr) => JsonConvert.DeserializeObject<T>(jsonStr)!;

    public static bool IsNullOrEmpty(this string str) =>
        string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);

    public static async Task EnsureTopicExistsAsync(string bootstrapServers, string topicName)
    {
        using var adminClient = new AdminClientBuilder(
            new AdminClientConfig { BootstrapServers = bootstrapServers }
        ).Build();

        try
        {
            var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
            if (metadata.Topics.Any(t => t.Topic == topicName))
                return;

            await adminClient.CreateTopicsAsync(
                new TopicSpecification[]
                {
                    new TopicSpecification
                    {
                        Name = topicName,
                        NumPartitions = 1,
                        ReplicationFactor = 1,
                    },
                }
            );
        }
        catch (CreateTopicsException e)
            when (e.Results.Any(r => r.Error.Code == ErrorCode.TopicAlreadyExists))
        {
            // Topic already exists, ignore
        }
    }
}
