using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Api.Redis;

public class RedisConfiguration
{
    public string Host { get; init; } = string.Empty;
    public int Port { get; set; }
}

public static class RedisExtension
{
    public static IServiceCollection AddRedis(this IServiceCollection services)
    {
        services.AddOptions<RedisConfiguration>()
            .BindConfiguration(nameof(RedisConfiguration))
            .Validate(config =>
            {
                if (string.IsNullOrEmpty(config.Host))
                {
                    throw new ValidationException($"{nameof(RedisConfiguration.Host)} is empty");
                }

                if (config.Port == 0)
                {
                    throw new ValidationException($"{nameof(RedisConfiguration.Port)} is not set");
                }

                return true;
            })
            .ValidateOnStart();

        services.AddSingleton<IRedisProvider>(s =>
        {
            var options = s.GetRequiredService<IOptions<RedisConfiguration>>();
            var configuration = options.Value;
            
            var redis = ConnectionMultiplexer.Connect($"{configuration.Host}:{configuration.Port}");
            var db = redis.GetDatabase();
            
            return new RedisProvider(db);
        });

        return services;
    }
}

public interface IRedisProvider
{
    Task SetValue(string key, string value);
    Task<string> GetValue(string key);

}

public class RedisProvider : IRedisProvider
{
    private readonly IDatabaseAsync _database;
    
    public RedisProvider(IDatabaseAsync database)
    {
        _database = database;
    }
    
    public Task SetValue(string key, string value)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key), "Key is null or empty: " + key);
        }
        
        var redisKey = new RedisKey(key);
        var redisValue = new RedisValue(value);

        return _database.StringSetAsync(redisKey, redisValue);
    }

    public async Task<string> GetValue(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key), "Key is null or empty: " + key);
        }
        
        var redisKey = new RedisKey(key);
        var redisValue = await _database.StringGetAsync(redisKey);
        return redisValue.ToString();
    }
}