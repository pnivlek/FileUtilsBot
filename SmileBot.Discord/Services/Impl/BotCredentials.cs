using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using NLog;

namespace SmileBot.Discord.Services.Impl
{
    public class BotCredentials : IBotCredentials
    {
        private Logger _log;

        public string Token { get; }

        public DbConfig Db { get; }

        private readonly string _credsFileName = Path.Combine(Directory.GetCurrentDirectory(), "Config/credentials.json");

        public BotCredentials()
        {
            _log = LogManager.GetCurrentClassLogger();
            try
            {
                var configBuilder = new ConfigurationBuilder();
                configBuilder.AddJsonFile(_credsFileName, true);

                var data = configBuilder.Build();
                Token = data[nameof(Token)];

                if (string.IsNullOrWhiteSpace(Token))
                {
                    _log.Error("Token is missing from credentials.json. Add it and restart the program.");
                    if (!Console.IsInputRedirected)
                        Console.ReadKey();
                    Environment.Exit(3);
                }

                var dbSection = data.GetSection("db");
                Db = new DbConfig(string.IsNullOrWhiteSpace(dbSection["Type"]) ?
                    "postgres" : dbSection["Type"],
                    string.IsNullOrWhiteSpace(dbSection["ConnectionString"]) ?
                    "postgres" : dbSection["ConnectionString"]);
            }
            catch (Exception ex)
            {
                _log.Fatal(ex.Message);
                _log.Fatal(ex);
                throw;
            }
        }
    }
}