using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Services
{
    public class RedisService
    {
        private readonly string _redisHost;
        private readonly string _redisPort;
        private ConnectionMultiplexer _redis;
        public IDatabase db { get; set; }
        public RedisService(IConfiguration configuration)
        {
            _redisHost = configuration["Redis:Host"];
            _redisPort = configuration["Redis:Port"];

            var configString = $"{_redisHost}:{_redisPort}";
            var config = new ConfigurationOptions
            {
                EndPoints = { configString },
                AbortOnConnectFail = false,
                ConnectTimeout = 10000,
                SyncTimeout = 10000 
            };
            _redis = ConnectionMultiplexer.Connect(config);
        }

        public IDatabase GetDb(int db)
        {
            return _redis.GetDatabase(db);
        }
    }
}
