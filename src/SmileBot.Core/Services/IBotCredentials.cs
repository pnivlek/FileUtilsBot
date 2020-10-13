using System;
using System.Collections.Generic;
using System.Text;

namespace SmileBot.Core.Services
{
    public interface IBotCredentials
    {
        string Token { get; }

        DbConfig Db { get; }
    }

    public class DbConfig
    {
        public DbConfig (string type, string connString)
        {
            this.Type = type;
            this.ConnectionString = connString;
        }

        public string Type { get; }
        public string ConnectionString { get; }
    }
}